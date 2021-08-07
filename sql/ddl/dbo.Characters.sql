CREATE TABLE Characters
(
	Id INT NOT NULL IDENTITY(1,1) PRIMARY KEY, 
    Name VARCHAR(20) NOT NULL, 
    Position CHAR(1) NULL, 
    Hp INT NOT NULL, 
    MaxHp INT NOT NULL, 
    Charge INT NOT NULL DEFAULT 0, 
    Defense INT NOT NULL DEFAULT 0, 
    Action INT NULL, 
    Target INT NULL,
	CONSTRAINT FK_Target_Character FOREIGN KEY (Target) REFERENCES Characters (Id),
    CONSTRAINT FK_Action FOREIGN KEY (Action) REFERENCES Actions (Id)
)
