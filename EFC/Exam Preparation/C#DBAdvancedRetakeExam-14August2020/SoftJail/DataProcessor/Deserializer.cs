namespace SoftJail.DataProcessor
{

    using Data;
    using Newtonsoft.Json;
    using SoftJail.Data.Models;
    using SoftJail.Data.Models.Enums;
    using SoftJail.DataProcessor.ImportDto;
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Globalization;
    using System.IO;
    using System.Linq;
    using System.Text;
    using System.Xml.Serialization;

    public class Deserializer
    {
        private const string ErrorMessage = "Invalid Data";
        private const string SuccessfullyAddedDepartments = "Imported {0} with {1} cells";
        private const string SuccessfullyAddedPrisoners = "Imported {0} {1} years old";
        private const string SuccessfullyAddedOfficers = "Imported {0} ({1} prisoners)";
        public static string ImportDepartmentsCells(SoftJailDbContext context, string jsonString)
        {
            var departmentsDto = JsonConvert.DeserializeObject<DepartmentCellImport[]>(jsonString);
            List<Department> departments = new List<Department>();
            StringBuilder sb = new StringBuilder();
            foreach (var department in departmentsDto)
            {
                if (!IsValid(department))
                {
                    sb.AppendLine(ErrorMessage);
                    continue;
                }

                if (department.Cells.Length == 0)
                {
                    sb.AppendLine(ErrorMessage);
                    continue;
                }
                Department departmentToAdd = new Department()
                {
                    Name = department.Name
                };

                bool cellNumberInvalid = false;
                foreach (var cell in department.Cells)
                {
                    if (!IsValid(cell))
                    {
                        sb.AppendLine(ErrorMessage);
                        cellNumberInvalid = true;
                        break;
                    }
                    departmentToAdd.Cells.Add(new Cell()
                    {
                        CellNumber = cell.CellNumber,
                        HasWindow = cell.HasWindow
                    });
                }
                if (cellNumberInvalid)
                {
                    continue;
                }

                sb.AppendLine(string.Format(SuccessfullyAddedDepartments, department.Name, departmentToAdd.Cells.Count));
                departments.Add(departmentToAdd);
            }
            context.Departments.AddRange(departments);
            context.SaveChanges();
            return sb.ToString().TrimEnd();
        }

        public static string ImportPrisonersMails(SoftJailDbContext context, string jsonString)
        {
            var prisonersDto = JsonConvert.DeserializeObject<PrisonerMailImport[]>(jsonString);
            List<Prisoner> prisoners = new List<Prisoner>();
            StringBuilder sb = new StringBuilder();
            foreach (var prisoner in prisonersDto)
            {
                if (!IsValid(prisoner))
                {
                    sb.AppendLine(ErrorMessage);
                    continue;
                }

                var isDateValid = DateTime.TryParseExact(prisoner.IncarcerationDate, "dd/MM/yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime date);
                if (!isDateValid)
                {
                    sb.AppendLine(ErrorMessage);
                    continue;
                }

                DateTime? releaseDate = null;
                if (!string.IsNullOrEmpty(prisoner.ReleaseDate))
                {
                    var isReleaseDateValid = DateTime.TryParseExact(prisoner.ReleaseDate, "dd/MM/yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime rDate);
                    releaseDate = rDate;
                }
                Prisoner prisonerToAdd = new Prisoner()
                {
                    Age = prisoner.Age,
                    Bail = prisoner.Bail,
                    FullName = prisoner.FullName,
                    IncarcerationDate = date,
                    CellId = prisoner.CellId,
                    Nickname = prisoner.Nickname,
                    ReleaseDate = releaseDate
                };
                var invalidMail = false;
                foreach (var mail in prisoner.Mails)
                {
                    if (!IsValid(mail))
                    {
                        sb.AppendLine(ErrorMessage);
                        invalidMail = true;
                        break;
                    }
                    prisonerToAdd.Mails.Add(new Mail()
                    {
                        Address = mail.Address,
                        Sender = mail.Sender,
                        Description = mail.Description
                    });
                }
                if (invalidMail)
                {
                    continue;
                }
                sb.AppendLine(string.Format(SuccessfullyAddedPrisoners, prisoner.FullName, prisoner.Age));
                prisoners.Add(prisonerToAdd);
            }
            context.Prisoners.AddRange(prisoners);
            context.SaveChanges();
            return sb.ToString().TrimEnd();
        }

        public static string ImportOfficersPrisoners(SoftJailDbContext context, string xmlString)
        {
            XmlRootAttribute rootAttribute = new XmlRootAttribute("Officers");
            XmlSerializer serializer = new XmlSerializer(typeof(OfficerImport[]), rootAttribute);
            var reader = new StringReader(xmlString);
            var officersDto = serializer.Deserialize(reader) as OfficerImport[];
            List<Officer> officers = new List<Officer>();
            StringBuilder sb = new StringBuilder();
            foreach (var officer in officersDto)
            {
                if (!IsValid(officer))
                {
                    sb.AppendLine(ErrorMessage);
                    continue;
                }
                var positionEnums = new string[]
                {
                    Position.Guard.ToString(),
                    Position.Labour.ToString(),
                    Position.Overseer.ToString(),
                    Position.Watcher.ToString()
                };

                var weaponEnums = new string[]
                {
                    Weapon.Sniper.ToString(),
                    Weapon.Pistol.ToString(),
                    Weapon.ChainRifle.ToString(),
                    Weapon.FlashPulse.ToString(),
                    Weapon.Knife.ToString()
                };

                if (!positionEnums.Any(x => x == officer.Position))
                {
                    sb.AppendLine(ErrorMessage);
                    continue;
                }

                if (!weaponEnums.Any(x => x == officer.Weapon))
                {
                    sb.AppendLine(ErrorMessage);
                    continue;
                }
                Officer officerToAdd = new Officer()
                {
                    Position = (Position)Enum.Parse(typeof(Position), officer.Position),
                    Weapon = (Weapon)Enum.Parse(typeof(Weapon), officer.Weapon),
                    DepartmentId = officer.DepartmentId,
                    FullName = officer.Name,
                    Salary = officer.Money,
                };

                List<OfficerPrisoner> op = new List<OfficerPrisoner>();
                foreach (var prisoner in officer.Prisoners)
                {
                    op.Add(new OfficerPrisoner()
                    {
                        Officer = officerToAdd,
                        PrisonerId = prisoner.Id
                    });
                }
                officerToAdd.OfficerPrisoners = op;

                sb.AppendLine(string.Format(SuccessfullyAddedOfficers, officer.Name, op.Count));
                officers.Add(officerToAdd);
            }
            context.Officers.AddRange(officers);
            context.SaveChanges();
            return sb.ToString().TrimEnd();
        }

        private static bool IsValid(object obj)
        {
            var validationContext = new System.ComponentModel.DataAnnotations.ValidationContext(obj);
            var validationResult = new List<ValidationResult>();

            bool isValid = Validator.TryValidateObject(obj, validationContext, validationResult, true);
            return isValid;
        }
    }
}