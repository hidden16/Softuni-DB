SELECT [MountainRange], [PeakName], [Elevation] FROM [Mountains], [Peaks]
WHERE [MountainRange] = 'Rila' AND [MountainId] = 17
ORDER BY [Elevation] DESC
