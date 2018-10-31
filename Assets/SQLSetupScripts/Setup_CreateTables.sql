--This sql script is to create all of the tables needed to save data for Byte. Database BYTEDB must be created before running this.

Use BYTEDB
Create Table SAVEFILE
(
	SaveFileId uniqueidentifier primary key default NEWID(),
	CreatedUtcDate datetime,
	CreateLocalTime datetime,
	Difficulty varchar(20),
	ScreenShotFilePath varchar(max),
	Playtime time
)

Create Table Item
(
	ItemId uniqueIdentifier primary key,
	ItemName varchar(max),
	BuyValue int,
	SellValue int,
	ItemDescription varchar(max),
	Notes varchar(max),
	Category varchar(20)
)

Create Table Inventory
(
	SaveFileId uniqueidentifier foreign key references SAVEFILE(SaveFileId),
	ItemId uniqueidentifier foreign key references Item(ItemId),
	Quantity int
)

Create Table GameLevel
(
	LevelId uniqueidentifier primary key,
	SceneName varchar(max),
	FilePath varchar(max),
)

Create Table SavePoint
(
	SavePointId uniqueidentifier primary key,
	SavePointName varchar(max),
	SpawnPosX decimal(5,2),
	SpawnPosY decimal(5,2),
	SpawnPosZ decimal(5,2),
	SpawnRotX decimal(5,2),
	SpawnRotY decimal(5,2),
	SpawnRotZ decimal(5,2)
)

Create Table GameTriggers
(
	TriggerId uniqueidentifier primary key,
	TriggerName varchar(max),
	TriggerDescription varchar(max)
)

Create Table SavedTriggers
(
	SaveFileId uniqueidentifier foreign key references SAVEFILE(SaveFileId),
	TriggerId uniqueidentifier foreign key references GameTriggers(TriggerId),
	Activated bit
)

Create Table LevelTriggers
(
	LevelId uniqueidentifier foreign key references GameLevel(LevelId),
	TriggerId uniqueidentifier foreign key references GameTriggers(TriggerId)
)

Create Table Weapons
(
	WeaponId uniqueidentifier primary key,
	WeaponName varchar(max),
	WeapomDescription varchar(max),
	WeaponInformation varchar(max),
	WeaponDamage int,
	Category varchar(20)
)

Create Table Skill
(
	SkillId uniqueidentifier primary key,
	SkillName varchar(max),
	SkillDescription varchar(max)
)

Create Table Upgrade
(
	UpgradeId uniqueidentifier primary key,
	UpgradeName varchar(max),
	UpgradeDescription varchar(max)
)

Create Table PlayerStats
(
	SaveFileId uniqueidentifier foreign key references SAVEFILE(SaveFileId),
	MaxHealth int,
	CurHealth int,
	MaxEnergy int,
	CurEnergy int,
	MaxVirus int,
	CurVirus int,
	EquippedWeaponId uniqueidentifier foreign key references Weapons(WeaponId),
	EquippedSkillId uniqueidentifier foreign key references Skill(SkillId)
)

Create Table SavedWeapons
(
	SaveFileId uniqueidentifier foreign key references SAVEFILE(SaveFileId),
	WeaponId uniqueidentifier foreign key references Weapons(WeaponId),
	HasWeapon bit
)

Create Table SavedSkills
(
	SaveFileId uniqueidentifier foreign key references SAVEFILE(SaveFileId),
	SkillId uniqueidentifier foreign key references Skill(SkillId),
	HasSkill bit
)

Create Table SavedUpgrades
(
	SaveFileId uniqueidentifier foreign key references SAVEFILE(SaveFileId),
	UpgradeId uniqueidentifier foreign key references Upgrade(UpgradeId),
	HasUpgrade bit
)

ALTER TABLE SAVEFILE
ADD LevelId uniqueidentifier,
	SavePointId uniqueidentifier