CREATE TABLE Actions
(
	Id INT NOT NULL IDENTITY(1,1) PRIMARY KEY,
	Action VARCHAR(10) NOT NULL,
	IsPlayerAction BIT NOT NULL -- TRUE if and only if it can be chosen as a player action.
)