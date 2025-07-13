create database QMS
----------- Quiz Management System - Tables & Strored Procedures -----------------

--MST_User Table:

CREATE TABLE  MST_User (
				UserID						INT IDENTITY(1,1) PRIMARY KEY, 
				UserName					NVARCHAR(100) NOT NULL,      
				Password					NVARCHAR(100) NOT NULL,      
				Email						NVARCHAR(100) NOT NULL,         
				Mobile						NVARCHAR(100) NOT NULL, 
				IsActive					BIT NOT NULL DEFAULT 1,
				IsAdmin						BIT NOT NULL DEFAULT 0,      
				Created						DATETIME DEFAULT GETDATE(),   
				Modified					DATETIME NOT NULL            
)

--MST_Quiz Table

CREATE TABLE MST_Quiz (
				QuizID						INT IDENTITY(1,1) PRIMARY KEY,
				QuizName					NVARCHAR(100) NOT NULL,
				TotalQuestions				INT NOT NULL,
				QuizDate					DATETIME NOT NULL,
				UserID						INT FOREIGN KEY REFERENCES  MST_User(UserID) NOT NULL,
				Created						DATETIME DEFAULT GETDATE() NOT NULL,
				Modified					DATETIME NOT NULL,
)

--MST_Question Table:

CREATE TABLE MST_Question (
				  QuestionID					INT IDENTITY(1,1) PRIMARY KEY,
				  QuestionText					NVARCHAR(MAX) NOT NULL,
				  QuestionLevelID				INT FOREIGN KEY REFERENCES MST_QuestionLevel(QuestionLevelID) NOT NULL,
				  OptionA						NVARCHAR(100) NOT NULL,
				  OptionB						NVARCHAR(100) NOT NULL,
				  OptionC						NVARCHAR(100) NOT NULL,
				  OptionD						NVARCHAR(100) NOT NULL,
				  CorrectOption					NVARCHAR(100) NOT NULL,
				  QuestionMarks					INT NOT NULL,
				  IsActive						BIT NOT NULL DEFAULT 1,
				  UserID						INT  FOREIGN KEY REFERENCES MST_User(UserID) NOT NULL,
				  Created						DATETIME DEFAULT GETDATE(),
				  Modified						DATETIME NOT NULL,
)
--MST_QuestionLevel Table:

CREATE TABLE MST_QuestionLevel (
				QuestionLevelID						INT IDENTITY(1,1) PRIMARY KEY,
				QuestionLevel						NVARCHAR(100) NOT NULL,
				UserID								INT FOREIGN KEY REFERENCES MST_User(UserID) NOT NULL,
				Created								DATETIME DEFAULT GETDATE() NOT NULL,
				Modified							DATETIME NOT NULL,
)

--MST_QuizWiseQuestions Table:

CREATE TABLE MST_QuizWiseQuestions (
				 QuizWiseQuestionsID			INT IDENTITY(1,1) PRIMARY KEY,
				 QuizID									INT FOREIGN KEY REFERENCES MST_Quiz(QuizID) NOT NULL,
				 QuestionID								INT FOREIGN KEY REFERENCES MST_Question(QuestionID) NOT NULL,
				 UserID									INT FOREIGN KEY REFERENCES MST_User(UserID) NOT NULL,
				 Created								DATETIME DEFAULT GETDATE() NOT NULL,
				 Modified								DATETIME NOT NULL,
)



--------- MST_User Table Procedures ----------
--  Retrieves all records from MST_User table.

--exec PR_MST_QMS_User_SelectAll
CREATE OR ALTER PROCEDURE [dbo].[PR_MST_User_SelectAll]
AS
BEGIN
		SELECT	[dbo].[MST_User].[UserID],
					[dbo].[MST_User].[UserName],
					[dbo].[MST_User].[Password],
					[dbo].[MST_User].[Email],
					[dbo].[MST_User].[Mobile],
					[dbo].[MST_User].[IsActive],
					[dbo].[MST_User].[IsAdmin],
					[dbo].[MST_User].[Created],
					[dbo].[MST_User].[Modified]

					FROM [dbo].[MST_User]
					INNER JOIN [DBO].[MST_Question]
					ON [DBO].[MST_Question].[UserID]=[DBO].[MST_User].[UserID]
					INNER JOIN  [DBO].[MST_QuestionLevel]
					ON [DBO].[MST_QuestionLevel].[UserID]=[DBO].[MST_User].[UserID]
					INNER JOIN  [DBO].[MST_Quiz]
					ON [DBO].[MST_Quiz].[UserID]=[DBO].[MST_User].[UserID]
					INNER JOIN  [DBO].[MST_QuizWiseQuestions]
					ON [DBO].[MST_QuizWiseQuestions].[UserID]=[DBO].[MST_User].[UserID]
					ORDER BY [DBO].[MST_User].[UserName]
END

-- Retrieves a specific user record based on primary key (UserID).

--exec PR_MST_User_SelectByPK (UserID)
create or alter procedure [dbo].[PR_MST_User_SelectByPK]
@UserID int
as
begin
			select	[dbo].[MST_User].[UserID],
						[dbo].[MST_User].[UserName],
						[dbo].[MST_User].[Password],
						[dbo].[MST_User].[Email],
						[dbo].[MST_User].[Mobile],
						[dbo].[MST_User].[IsActive],
						[dbo].[MST_User].[IsAdmin],
						[dbo].[MST_User].[Created],
						[dbo].[MST_User].[Modified]

						from [MST_User]
						where [MST_User].[UserID] = @UserID
end

-- Inserts a new user record into MST_User table.

-- exec PR_MST_User_Insert
create or alter procedure [dbo].[PR_MST_User_Insert]
		@username		Nvarchar(100),
		@password		Nvarchar(100),
		@email			Nvarchar(100),
		@mobile			Nvarchar(100),
		@isActive		bit,
		@isAdmin		bit
as
begin
		insert into [dbo].[MST_User]
		(
			[UserName],
			[Password],
			[Email],
			[Mobile],
			[IsActive],
			[IsAdmin],
			[Modified]
		)
		values
		(
			@username,
			@password,
			@email,
			@mobile,
			@isActive,
			@isAdmin,
			GETDATE()
		)
end

-- Updates specific fields of a user record based on primary key (UserID).

--exec PR_MST_User_Update
create or alter procedure [dbo].[PR_MST_User_Update]
		@userID				int,
		@username			Nvarchar(100),
		@password			Nvarchar(100),
		@email				Nvarchar(100),
		@mobile				Nvarchar(100),
		@isActive			bit,
		@isAdmin			bit
as
begin
		Update [dbo].[MST_User] set
		[UserName]=@username,
		[Password]=@password,
		[Email]	=@email,
		[Mobile]=@mobile,
		[IsActive]=@isActive,
		[IsAdmin]=@isAdmin,
		[Modified]=GETDATE()

		where [dbo].[MST_User].[UserID] = @userID
end


-- Deletes a user record from MST_User table based on primary key (UserID).

--exec PR_MST_User_Delete
create or alter procedure [dbo].[PR_MST_User_Delete]
@userID int
as
begin
		delete
		from [dbo].[MST_User]
		where [dbo].[MST_User].[UserID] = @userID
end

 -------- MST_Quiz Table Procedures --------
-- Retrieves all records from MST_Quiz table.

--exec PR_MST_Quiz_SelectAll
create or alter procedure [dbo].[PR_MST_Quiz_SelectAll]
as
begin
		select	[dbo].[MST_Quiz].[QuizID],
					[dbo].[MST_Quiz].[QuizName],
					[dbo].[MST_Quiz].[TotalQuestions],
					[dbo].[MST_Quiz].[QuizDate],
					[dbo].[MST_Quiz].[UserID],
					[dbo].[MST_Quiz].[Created],
					[dbo].[MST_Quiz].[Modified]
				
					
					FROM [dbo].[MST_Quiz]
					INNER JOIN  [DBO].[MST_QuizWiseQuestions]
					ON [DBO].[MST_QuizWiseQuestions].[UserID]=[DBO].[MST_Quiz].[UserID]
					ORDER BY [DBO].[MST_Quiz].[QuizName]

end

-- Retrieves a specific quiz record based on primary key (QuizID).

--exec PR_MST_Quiz_SelectByPK
create or alter procedure [dbo].[PR_MST_Quiz_SelectByPK]
@quizID		int
as
begin
		select	[dbo].[MST_Quiz].[QuizID],
					[dbo].[MST_Quiz].[QuizName],
					[dbo].[MST_Quiz].[TotalQuestions],
					[dbo].[MST_Quiz].[QuizDate],
					[dbo].[MST_Quiz].[UserID],
					[dbo].[MST_Quiz].[Created],
					[dbo].[MST_Quiz].[Modified]

					from [dbo].[MST_Quiz]

					where [dbo].[MST_Quiz].[QuizID] = @quizID
end

-- Inserts a new quiz record into MST_Quiz table.

-- exec PR_MST_Quiz_Insert
create or alter procedure [dbo].[PR_MST_Quiz_Insert]
		@quizName			Nvarchar(100),
		@totalQuestions		int,
		@quizDate			datetime,
		@userID				int
as
begin
		insert into [dbo].[MST_Quiz]
		(
			[QuizName],
			[TotalQuestions],
			[QuizDate],
			[UserID],
			[Modified]
		)
		values
		(
			@quizName,
			@totalQuestions,
			@quizDate,
			@userID,
			GETDATE()
		)
end

-- Updates specific fields of a quiz record based on primary key (QuizID).

--exec PR_MST_Quiz_Update
create or alter procedure [dbo].[PR_MST_Quiz_Update]
		@quizID					int,
		@quizName			Nvarchar(100),
		@totalQuestions		int,
		@quizDate				datetime,
		@userID					int
as
begin
		update [dbo].[MST_Quiz] set
		[QuizName]=@quizName,
		[TotalQuestions]=@totalQuestions,
		[QuizDate]=@quizDate,
		[UserID]=@userID,
		[Modified]=GETDATE()

		where [dbo].[MST_Quiz].[QuizID] = @quizID
end

--  Deletes a quiz record from MST_Quiz table based on primary key (QuizID).

-- exec PR_MST_Quiz_Delete
create or alter procedure [dbo].[PR_MST_Quiz_Delete]
@quizID		int
as
begin
			delete
			from [dbo].[MST_Quiz]
			where [dbo].[MST_Quiz].[QuizID] = @quizID
end

----------- MST_Question Table Procedures ------------
--Retrieves all records from MST_Question table.
select * from MST_Question
--exec PR_MST_Question_SelectAll
USE [QMS]
GO
/****** Object:  StoredProcedure [dbo].[PR_MST_Question_SelectAll]    Script Date: 21-02-2025 09:24:07 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
create or ALTER   PROCEDURE [dbo].[PR_MST_Question_SelectAll]
	AS
	BEGIN
		SELECT 
			[dbo].[MST_Question].[QuestionID],
			[dbo].[MST_Question].[QuestionText],
			[dbo].[MST_Question].[OptionA],
			[dbo].[MST_Question].[OptionB],
			[dbo].[MST_Question].[OptionC],
			[dbo].[MST_Question].[OptionD],
			[dbo].[MST_Question].[CorrectOption],
			[dbo].[MST_Question].[QuestionMarks],
			[dbo].[MST_Question].[IsActive],
			[dbo].[MST_Question].[QuestionLevelID],
			[dbo].[MST_QuestionLevel].[QuestionLevel],
			[dbo].[MST_Question].[UserID],
			[dbo].[MST_User].[UserName],
			[dbo].[MST_Question].[Created],
			[dbo].[MST_Question].[Modified]
		FROM [dbo].[MST_Question]
		
		JOIN [dbo].[MST_User]
		ON [dbo].[MST_Question].[UserID] = [dbo].[MST_User].[UserID]
		
		JOIN [dbo].[MST_QuestionLevel]
		ON [dbo].[MST_Question].[QuestionLevelID] = [dbo].[MST_QuestionLevel].[QuestionLevelID]
		
		ORDER BY [dbo].[MST_Question].[QuestionID],[dbo].[MST_QuestionLevel].[QuestionLevel],[dbo].[MST_User].[UserName]
	END

--Retrieves a specific question record based on primary key (QuestionID).

-- exec PR_MST_Question_SelectByPK
create or alter procedure [dbo].[PR_MST_Question_SelectByPK]
	@questionID		int
as
begin
		select	[dbo].[MST_Question].[QuestionID],
					[dbo].[MST_Question].[QuestionText],
					[dbo].[MST_Question].[QuestionLevelID],
					[dbo].[MST_Question].[OptionA],
					[dbo].[MST_Question].[OptionB],
					[dbo].[MST_Question].[OptionC],
					[dbo].[MST_Question].[OptionD],
					[dbo].[MST_Question].[CorrectOption],
					[dbo].[MST_Question].[QuestionMarks],
					[dbo].[MST_Question].[IsActive],
					[dbo].[MST_Question].[UserID],
					[dbo].[MST_Question].[Created],
					[dbo].[MST_Question].[Modified]

					from [dbo].[MST_Question]
					where [dbo].[MST_Question].[QuestionID]	= @questionID
end

--Inserts a new question record into MST_Question table.

-- exec PR_MST_Question_Insert
create or alter procedure [dbo].[PR_MST_Question_Insert]
		@questionText		Nvarchar(max),
		@questionlevelID	int,
		@optionA			Nvarchar(100),
		@optionB			Nvarchar(100),
		@optionC			Nvarchar(100),
		@optionD			Nvarchar(100),
		@correctOption		Nvarchar(100),
		@questionMarks		int,
		@isActive			bit,
		@userID				int
as
begin
		insert into [dbo].[MST_Question]
		(
			[QuestionText],
			[QuestionLevelID],
			[OptionA],
			[OptionB],
			[OptionC],
			[OptionD],
			[CorrectOption],
			[QuestionMarks],
			[IsActive],
			[UserID],
			[Modified]
		)
		values
		(
			@questionText,
			@questionlevelID,
			@optionA,
			@optionB,
			@optionC,
			@optionD,
			@correctOption,
			@questionMarks,
			@isActive,
			@userID,
			GETDATE()
		)
end

-- Updates specific fields of a question record based on primary key (QuestionID).

-- exec PR_MST_Question_Update
create or alter procedure [dbo].[PR_MST_Question_Update]
		@questionID			int,
		@questionText		Nvarchar(max),
		@questionlevelID	int,
		@optionA			Nvarchar(100),
		@optionB			Nvarchar(100),
		@optionC			Nvarchar(100),
		@optionD			Nvarchar(100),
		@correctOption		Nvarchar(100),
		@questionMarks		int,
		@isActive			bit,
		@userID				int
as
begin
		update [dbo].[MST_Question]  set
		[QuestionText]	=	@questionText,
		[QuestionLevelID]=	@questionlevelID,
		[OptionA]		=	@optionA,
		[OptionB]		=	@optionB,
		[OptionC]		=	@optionC,
		[OptionD]		=	@optionD,
		[CorrectOption]	=	@correctOption,
		[QuestionMarks]	=	@questionMarks,
		[IsActive]		=	@isActive,
		[UserID]		=	@userID,
		[Modified]		=	GETDATE()

		where [dbo].[MST_Question].[QuestionID] = @questionID
end

--Deletes a question record from MST_Question table based on primary key (QuestionID).

-- exec PR_MST_Question_Delete
create or alter procedure	 [dbo].[PR_MST_Question_Delete]
	@questionID		int
as
begin
		delete
		from [dbo].[MST_Question]
		where [dbo].[MST_Question].[QuestionID]	= @questionID
end

---------- MST_QuestionLevel Table Procedures -----------

-- Retrieves all records from MST_QuestionLevel table.

--exec PR_MST_QuestionLevel_SelectAll
create or alter procedure [dbo].[PR_MST_QuestionLevel_SelectAll]
as
begin
		select	[dbo].[MST_QuestionLevel].[QuestionLevelID],
					[dbo].[MST_QuestionLevel].[QuestionLevel],
					[dbo].[MST_QuestionLevel].[UserID],
					[dbo].[MST_QuestionLevel].[Created],
					[dbo].[MST_QuestionLevel].[Modified]

					from [dbo].[MST_QuestionLevel]
					INNER JOIN [DBO].[MST_User]
					ON [DBO].[MST_User].[UserID]=[DBO].[MST_QuestionLevel].[UserID]
					ORDER BY [dbo].[MST_QuestionLevel].[QuestionLevelID]

end

-- Retrieves a specific question level record based on primary key (QuestionLevelID).

-- exec PR_MST_QuestionLevel_SelectByPK
create or alter procedure [dbo].[PR_MST_QuestionLevel_SelectByPK]
	@questionLevelID		int
as
begin
		select	[dbo].[MST_QuestionLevel].[QuestionLevelID],
					[dbo].[MST_QuestionLevel].[QuestionLevel],
					[dbo].[MST_QuestionLevel].[UserID],
					[dbo].[MST_QuestionLevel].[Created],
					[dbo].[MST_QuestionLevel].[Modified]

					from [dbo].[MST_QuestionLevel]
					where [dbo].[MST_QuestionLevel].[QuestionLevelID] = @questionLevelID
end

-- Inserts a new question level record into MST_QuestionLevel table.

-- exec PR_MST_QuestionLevel_Insert
create or alter procedure [dbo].[PR_MST_QuestionLevel_Insert]
		@questionLevel		Nvarchar(100),
		@userId					int
as
begin
		insert into [dbo].[MST_QuestionLevel]
		(
			[QuestionLevel],
			[UserID],
			[Modified]
		)
		values
		(
			@questionLevel,
			@userId,
			GETDATE()
		)
end

-- Updates specific fields of a question level record based on primary key (QuestionLevelID).

-- exec PR_MST_QuestionLevel_Update
create or alter procedure [dbo].[PR_MST_QuestionLevel_Update]
		@questionLevelID	int,
		@questionLevel		Nvarchar(100),
		@userId				int
as
begin
		Update [dbo].[MST_QuestionLevel] set
		[QuestionLevel]		=	@questionLevel,
		[UserID]			=	@userId,
		[Modified]			=	GETDATE()
end

-- Deletes a question level record from MST_QuestionLevel table based on primary key (QuestionLevelID).

-- exec PR_MST_QuestionLevel_Delete
create or alter procedure [dbo].[PR_MST_QuestionLevel_Delete]
		@questionLevelID		int
as
begin
		delete
		from [dbo].[MST_QuestionLevel]
		where [dbo].[MST_QuestionLevel].[QuestionLevelID] = @questionLevelID
end

--------------- MST_QuizWiseQuestions Table Procedures ------------

-- Retrieves all records from MST_QuizWiseQuestions table.

-- exec PR_MST_QuizWiseQuestions_SelectAll
create or alter procedure [dbo].[PR_MST_QuizWiseQuestions_SelectAll]
as
begin
		select	[dbo].[MST_QuizWiseQuestions].[QuizWiseQuestionsID],
					[dbo].[MST_QuizWiseQuestions].[QuizID],
					[dbo].[MST_QuizWiseQuestions].[QuestionID],
					[dbo].[MST_QuizWiseQuestions].[UserID],
					[dbo].[MST_QuizWiseQuestions].[Created],
					[dbo].[MST_QuizWiseQuestions].[Modified]

					from [dbo].[MST_QuizWiseQuestions]
					INNER JOIN [DBO].[MST_Question]
					ON [DBO].[MST_Question].[QuestionID]=[DBO].[MST_QuizWiseQuestions].[QuestionID]
					INNER JOIN [DBO].[MST_User]
					ON [DBO].[MST_User].[UserID]=[DBO].[MST_QuizWiseQuestions].[UserID]
					ORDER BY [DBO].[MST_QuizWiseQuestions].QuizWiseQuestionsID
end

-- Retrieves a specific quiz-question mapping based on primary key (QuizWiseQuestionsID).

-- exec PR_MST_QuizWiseQuestions_SelectByPK
create or alter procedure [dbo].[PR_MST_QuizWiseQuestions_SelectByPK]
		@quizWiseQuestionsID		int
as
begin
		select	[dbo].[MST_QuizWiseQuestions].[QuizWiseQuestionsID],
					[dbo].[MST_QuizWiseQuestions].[QuizID],
					[dbo].[MST_QuizWiseQuestions].[QuestionID],
					[dbo].[MST_QuizWiseQuestions].[UserID],
					[dbo].[MST_QuizWiseQuestions].[Created],
					[dbo].[MST_QuizWiseQuestions].[Modified]

					from [dbo].[MST_QuizWiseQuestions]
					where [dbo].[MST_QuizWiseQuestions].[QuizWiseQuestionsID] = @quizWiseQuestionsID
end

-- Inserts a new quiz-question mapping record into MST_QuizWiseQuestions table.

-- exec PR_MST_QuizWiseQuestions_Insert
create or alter procedure [PR_MST_QuizWiseQuestions_Insert]
		@quizID			int,
		@questionID		int,
		@userID			int
as
begin
		insert into [dbo].[MST_QuizWiseQuestions]
		(
			[QuizID],
			[QuestionID],
			[UserID],
			[Modified]
		)
		values
		(
			@quizID,
			@questionID,
			@userID,
			GETDATE()
		)
end

--Updates specific fields of a quiz-question mapping record based on primary key (QuizWiseQuestionsID).

-- exec PR_MST_QuizWiseQuestions_Update
create or alter procedure [dbo].[PR_MST_QuizWiseQuestions_Update]
		@quizWiseQuestionsId		int,
		@quizID						int,
		@questionID					int,
		@userID						int
as
begin
		Update [dbo].[MST_QuizWiseQuestions] set
		[QuizID]	=@quizID,
		[QuestionID]=@questionID,
		[UserID]	=@userID,
		[Modified]	=GETDATE()

		where [dbo].[MST_QuizWiseQuestions].[QuizWiseQuestionsID] = @quizWiseQuestionsId
end
-- Deletes a quiz-question mapping record from MST_QuizWiseQuestions table based on primary key (QuizWiseQuestionsID).

-- exec PR_MST_QuizWiseQuestions_Delete
create or alter procedure [dbo].[PR_MST_QuizWiseQuestions_Delete]
		@quizWiseQuestionsID		int
as
begin
		delete
		from [dbo].[MST_QuizWiseQuestions]
		where [dbo].[MST_QuizWiseQuestions].[QuizWiseQuestionsID] = @quizWiseQuestionsID
end

--------------- Login Authentication Procedure for MST_User ------------------
-- Authenticate a user during login using username, email, or mobile number and password if the user is active.

-- exec PR_MST_User_SelectByUserNamePassword
create or alter procedure [dbo].[PR_MST_User_SelectByUserNamePassword]
		@Username			Nvarchar(100), 
		@Password			Nvarchar(100)
as
begin

	select	[dbo].[MST_User].[UserName],
				[dbo].[MST_User].[Password]
				
				from [dbo].[MST_User]
				where 
					( 
					[dbo].[MST_User].[UserName]			 =		@Username
					or
					[dbo].[MST_User].[Email]			 =		@Username
					or
					[dbo].[MST_User].[Mobile]			 =		@Username
					) 
					and
					[dbo].[MST_User].[Password]			=		@Password  
					and 
					[dbo].[MST_User].[IsActive]			 =		1
 end

 ----------------- Procedure to fetch QuestionLevel dropdown (in Question Table) data from MST_QuestionLevel table --------------------

 -- Fetches QuestionLevelID and QuestionLevel for dropdown population in the form.

-- exec PR_MST_QuestionLevel_FillDropdown
 create or alter procedure [dbo].[PR_MST_QuestionLevel_FillDropdown]
as
begin
		select	[dbo].[MST_QuestionLevel].[QuestionLevelID],
					[dbo].[MST_QuestionLevel].[QuestionLevel]

					from [dbo].[MST_QuestionLevel]
					order by [dbo].[MST_QuestionLevel].[QuestionLevel]
end

------- Procedure to Fetch All Quizzes for Quiz Dropdown in QuizWiseQuestions Form -----------

-- This procedure fetches all Quiz IDs and their corresponding Quiz Names from the MST_Quiz table.

-- exec PR_MST_QuizWiseQuestions_Fill_Quiz_Dropdown
create or alter procedure [dbo].[PR_MST_QuizWiseQuestions_Fill_Quiz_Dropdown]
as
begin
		select	 [dbo].[MST_Quiz].[QuizID],
					 [dbo].[MST_Quiz].[QuizName]

					 from [dbo].[MST_Quiz]
end

--------- Procedure to Fetch All Questions for Question Dropdown in QuizWiseQuestions Form---------------

-- This procedure fetches all the questions with their corresponding question level.

-- exec PR_MST_QuizWiseQuestions_Fill_Question_Dropdown
create or alter procedure [dbo].[PR_MST_QuizWiseQuestions_Fill_Question_Dropdown]
as
begin
		select	[dbo].[MST_QuestionLevel].[QuestionLevel] +  ' - '  +[dbo].[MST_Question].[QuestionText] as QuestionText,
					[dbo].[MST_Question].[QuestionID]
		from 
					[dbo].[MST_Question]
		join
					[dbo].[MST_QuestionLevel]
					on [dbo].[MST_Question].[QuestionLevelID] = [dbo].[MST_QuestionLevel].[QuestionLevelID]
end

create or alter proc [dbo].[PR_MST_Quiz_Dropdown]
as
begin
	select USERID,USERNAME from MST_User
	 order by USERNAME
end




