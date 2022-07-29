namespace Artillery.DataProcessor.ExportDto
{
    public class ShellExport
    {
        public double ShellWeight { get; set; }
        public string Caliber { get; set; }
        public ShellGunExport[] Guns { get; set; }
    }
    public class ShellGunExport
    {
        public string GunType { get; set; }
        public int GunWeight { get; set; }
        public double BarrelLength { get; set; }
        public string Range { get; set; }
    }
}
