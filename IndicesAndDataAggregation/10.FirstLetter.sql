SELECT *
FROM
(
SELECT
SUBSTRING(FirstName,1,1) AS FirstLetter
FROM WizzardDeposits
WHERE DepositGroup = 'Troll Chest'
) AS a
GROUP BY a.FirstLetter
ORDER BY FirstLetter ASC
