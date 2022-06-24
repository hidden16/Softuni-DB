SELECT
SUM(a.Diff) AS SumDifference
FROM
(
	SELECT
	Id,
	DepositAmount,
	DepositAmount - LEAD(DepositAmount,1) OVER (ORDER BY Id ASC) AS Diff
	FROM WizzardDeposits
) AS a
