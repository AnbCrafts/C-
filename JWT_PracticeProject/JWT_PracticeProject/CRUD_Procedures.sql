USE [AssignmentDB];
GO
select * from labTest.QueryUser
/****** 1. Table schema reference: labTest.QueryUser ******/
-- SELECT TOP (1000) [UserId], [UserName], [Email], [FullName], [MobileNo], [City], [StateName], [Country], [Gender], [QueryText], [CreatedDate], [IsActive], [QueryAmount]
-- FROM [AssignmentDB].[labTest].[QueryUser];

/****** 2. Stored Procedure for Login ******/
CREATE OR ALTER PROCEDURE labTest.loginUsers
    @username VARCHAR(100),
    @phone VARCHAR(100),
    @message VARCHAR(400) OUT
AS
BEGIN
    SET NOCOUNT OFF;
    SELECT UserId, UserName, Email, FullName, MobileNo, City, StateName, Country, Gender, QueryText, CreatedDate, IsActive, QueryAmount
    FROM labTest.QueryUser AS qu 
    WHERE qu.UserName = @username AND qu.MobileNo = @phone;
    
    IF @@ROWCOUNT < 1
        SET @message = 'User not found with these credentials';
    ELSE 
        SET @message = 'User found successfully';
END;
GO

/****** 3. Stored Procedure: Get All Users ******/
CREATE OR ALTER PROCEDURE [labTest].[sp_GetAllQueryUsers]
AS
BEGIN
    SET NOCOUNT ON;
    SELECT 
        UserId,
        UserName,
        Email,
        FullName,
        MobileNo,
        City,
        StateName,
        Country,
        Gender,
        QueryText,
        CreatedDate,
        IsActive,
        QueryAmount
    FROM [labTest].[QueryUser]
    ORDER BY UserId DESC;
END;
GO

/****** 4. Stored Procedure: Get User by ID ******/
CREATE OR ALTER PROCEDURE [labTest].[sp_GetQueryUserById]
    @UserId INT
AS
BEGIN
    SET NOCOUNT ON;
    SELECT 
        UserId,
        UserName,
        Email,
        FullName,
        MobileNo,
        City,
        StateName,
        Country,
        Gender,
        QueryText,
        CreatedDate,
        IsActive,
        QueryAmount
    FROM [labTest].[QueryUser]
    WHERE UserId = @UserId;
END;
GO

/****** 5. Stored Procedure: Insert User ******/
CREATE OR ALTER PROCEDURE [labTest].[sp_InsertQueryUser]
    @UserName VARCHAR(100),
    @Email VARCHAR(255),
    @FullName VARCHAR(150) = NULL,
    @MobileNo VARCHAR(15) = NULL,
    @City VARCHAR(100) = NULL,
    @StateName VARCHAR(100) = NULL,
    @Country VARCHAR(100) = NULL,
    @Gender VARCHAR(10) = NULL,
    @QueryText VARCHAR(MAX) = NULL,
    @IsActive BIT = 1,
    @QueryAmount DECIMAL(18,2) = NULL
AS
BEGIN
    SET NOCOUNT ON;
    BEGIN TRY
        INSERT INTO [labTest].[QueryUser]
        (
            UserName,
            Email,
            FullName,
            MobileNo,
            City,
            StateName,
            Country,
            Gender,
            QueryText,
            CreatedDate,
            IsActive,
            QueryAmount
        )
        VALUES
        (
            @UserName,
            @Email,
            @FullName,
            @MobileNo,
            @City,
            @StateName,
            @Country,
            @Gender,
            @QueryText,
            GETDATE(),
            ISNULL(@IsActive, 1),
            @QueryAmount
        );

        DECLARE @newId INT = SCOPE_IDENTITY();
        SELECT @newId AS NewUserId, 'User inserted successfully with ID ' + CAST(@newId AS VARCHAR(10)) AS Result;
    END TRY
    BEGIN CATCH
        SELECT -1 AS NewUserId, 'Error: ' + ERROR_MESSAGE() AS Result;
    END CATCH
END;
GO

/****** 6. Stored Procedure: Update User ******/
CREATE OR ALTER PROCEDURE [labTest].[sp_UpdateQueryUser]
    @UserId INT,
    @UserName VARCHAR(100),
    @Email VARCHAR(255),
    @FullName VARCHAR(150) = NULL,
    @MobileNo VARCHAR(15) = NULL,
    @City VARCHAR(100) = NULL,
    @StateName VARCHAR(100) = NULL,
    @Country VARCHAR(100) = NULL,
    @Gender VARCHAR(10) = NULL,
    @QueryText VARCHAR(MAX) = NULL,
    @IsActive BIT = 1,
    @QueryAmount DECIMAL(18,2) = NULL
AS
BEGIN
    SET NOCOUNT ON;
    BEGIN TRY
        IF NOT EXISTS (SELECT 1 FROM [labTest].[QueryUser] WHERE UserId = @UserId)
        BEGIN
            SELECT 0 AS RowsAffected, 'User with ID ' + CAST(@UserId AS VARCHAR(10)) + ' not found to update' AS Result;
            RETURN;
        END

        UPDATE [labTest].[QueryUser]
        SET
            UserName = @UserName,
            Email = @Email,
            FullName = @FullName,
            MobileNo = @MobileNo,
            City = @City,
            StateName = @StateName,
            Country = @Country,
            Gender = @Gender,
            QueryText = @QueryText,
            IsActive = ISNULL(@IsActive, 1),
            QueryAmount = @QueryAmount
        WHERE UserId = @UserId;

        SELECT 1 AS RowsAffected, 'User with ID ' + CAST(@UserId AS VARCHAR(10)) + ' updated successfully' AS Result;
    END TRY
    BEGIN CATCH
        SELECT 0 AS RowsAffected, 'Error: ' + ERROR_MESSAGE() AS Result;
    END CATCH
END;
GO

/****** 7. Stored Procedure: Delete User ******/
CREATE OR ALTER PROCEDURE [labTest].[sp_DeleteQueryUser]
    @UserId INT
AS
BEGIN
    SET NOCOUNT ON;
    BEGIN TRY
        IF NOT EXISTS (SELECT 1 FROM [labTest].[QueryUser] WHERE UserId = @UserId)
        BEGIN
            SELECT 0 AS RowsAffected, 'User with ID ' + CAST(@UserId AS VARCHAR(10)) + ' not found to delete' AS Result;
            RETURN;
        END

        DELETE FROM [labTest].[QueryUser]
        WHERE UserId = @UserId;

        SELECT 1 AS RowsAffected, 'User with ID ' + CAST(@UserId AS VARCHAR(10)) + ' deleted successfully' AS Result;
    END TRY
    BEGIN CATCH
        SELECT 0 AS RowsAffected, 'Error: ' + ERROR_MESSAGE() AS Result;
    END CATCH
END;
GO

/****** 8. Table: Refresh Tokens ******/
IF NOT EXISTS (SELECT * FROM sys.tables t JOIN sys.schemas s ON t.schema_id = s.schema_id WHERE s.name = 'labTest' AND t.name = 'UserRefreshTokens')
BEGIN
    CREATE TABLE [labTest].[UserRefreshTokens] (
        [Id] INT IDENTITY(1,1) PRIMARY KEY,
        [UserId] INT NOT NULL,
        [Token] NVARCHAR(500) NOT NULL,
        [ExpiresAt] DATETIME NOT NULL,
        [IsRevoked] BIT NOT NULL DEFAULT 0,
        [CreatedAt] DATETIME NOT NULL DEFAULT GETDATE(),
        [RevokedAt] DATETIME NULL
    );
    CREATE INDEX IX_UserRefreshTokens_Token ON [labTest].[UserRefreshTokens]([Token]);
END;
GO

/****** 9. Stored Procedures for Refresh Token Handling ******/
CREATE OR ALTER PROCEDURE [labTest].[sp_SaveRefreshToken]
    @UserId INT,
    @Token NVARCHAR(500),
    @ExpiresAt DATETIME
AS
BEGIN
    SET NOCOUNT ON;
    UPDATE [labTest].[UserRefreshTokens]
    SET IsRevoked = 1, RevokedAt = GETDATE()
    WHERE UserId = @UserId AND IsRevoked = 0;

    INSERT INTO [labTest].[UserRefreshTokens] (UserId, Token, ExpiresAt, IsRevoked, CreatedAt)
    VALUES (@UserId, @Token, @ExpiresAt, 0, GETDATE());
END;
GO

CREATE OR ALTER PROCEDURE [labTest].[sp_GetRefreshToken]
    @Token NVARCHAR(500)
AS
BEGIN
    SET NOCOUNT ON;
    SELECT Id, UserId, Token, ExpiresAt, IsRevoked, CreatedAt, RevokedAt
    FROM [labTest].[UserRefreshTokens]
    WHERE Token = @Token;
END;
GO

CREATE OR ALTER PROCEDURE [labTest].[sp_RevokeRefreshToken]
    @Token NVARCHAR(500)
AS
BEGIN
    SET NOCOUNT ON;
    UPDATE [labTest].[UserRefreshTokens]
    SET IsRevoked = 1, RevokedAt = GETDATE()
    WHERE Token = @Token;
END;
GO
