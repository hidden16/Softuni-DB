SELECT TOP 5
o.[Name]
,COUNT(*) AS CountOfAnimals
FROM Animals AS a
JOIN Owners AS o ON a.OwnerId = o.Id
GROUP BY o.[Name]
ORDER BY CountOfAnimals DESC, o.[Name]