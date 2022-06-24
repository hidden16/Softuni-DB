SELECT
a.[Name]
,aa.AnimalType
,FORMAT(a.BirthDate,'dd.MM.yyyy') AS BirthDate
FROM Animals AS a
JOIN AnimalTypes AS aa ON a.AnimalTypeId = aa.Id
ORDER BY a.[Name]