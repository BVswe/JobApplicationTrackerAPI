CREATE TABLE [dbo].[JobApplications]
(
	[Id] INT Identity(1,1) PRIMARY KEY,
	Company varchar(100),
	Position varchar(100),
	ApplicationStatus varchar(50),
)
