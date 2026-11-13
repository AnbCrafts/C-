

USE [AssignmentDB];
GO

-- 1. Create Schema 'labTest' if not exists
IF NOT EXISTS (SELECT * FROM sys.schemas WHERE name = 'labTest')
BEGIN
    EXEC('CREATE SCHEMA labTest');
END
GO

-- 2. Create Users Table (for JWT Auth /api/auth/login)
IF OBJECT_ID('labTest.Users', 'U') IS NULL
BEGIN
    CREATE TABLE labTest.Users
    (
        UserId          INT IDENTITY(1,1) PRIMARY KEY,
        Username        NVARCHAR(50)    NOT NULL UNIQUE,
        PasswordHash    NVARCHAR(256)   NOT NULL,
        Role            NVARCHAR(30)    NOT NULL DEFAULT 'Admin',
        IsActive        BIT             NOT NULL DEFAULT 1,
        CreatedAt       DATETIME2       NOT NULL DEFAULT GETDATE()
    );

    -- Seed Default Users
    INSERT INTO labTest.Users (Username, PasswordHash, Role, IsActive)
    VALUES 
        ('admin', 'Password123!', 'Admin', 1),
        ('tech_user', 'Tech@123', 'Technician', 1);
END
GO

-- 3. Create Assets Table (for /api/assets)
IF OBJECT_ID('labTest.Assets', 'U') IS NULL
BEGIN
    CREATE TABLE labTest.Assets
    (
        AssetId         INT IDENTITY(1,1) PRIMARY KEY,
        AssetName       NVARCHAR(100)   NOT NULL,
        Category        NVARCHAR(50)    NOT NULL,
        PurchaseDate    DATE            NOT NULL,
        Status          NVARCHAR(30)    NOT NULL,
        Price           DECIMAL(18, 2)  NOT NULL,
        CreatedAt       DATETIME2       NOT NULL DEFAULT GETDATE(),
        UpdatedAt       DATETIME2       NULL,

        CONSTRAINT CK_Assets_Price CHECK (Price >= 0),
        CONSTRAINT CK_Assets_Status CHECK (Status IN ('Active', 'In Use', 'Under Maintenance', 'Decommissioned', 'Deleted'))
    );

   
    INSERT INTO labTest.Assets (AssetName, Category, PurchaseDate, Status, Price)
    VALUES
        ('Dell Precision 3660 Workstation', 'Lab System', '2024-03-15', 'Active', 1450.00),
        ('Cisco Catalyst 2960-X Switch', 'Networking', '2023-11-20', 'In Use', 850.50),
        ('HP ProLiant DL380 Server', 'Server', '2024-01-10', 'Active', 3200.00),
        ('MikroTik Cloud Core Router CCR1009', 'Networking', '2023-08-05', 'Under Maintenance', 495.00),
        ('Lenovo ThinkVision 27" 4K Monitor', 'Lab System', '2024-05-12', 'Active', 320.00);
END
GO

-- ============================================================================
-- STORED PROCEDURES
-- ============================================================================

-- Procedure 1: sp_GetAllAssets
IF OBJECT_ID('labTest.sp_GetAllAssets', 'P') IS NOT NULL
    DROP PROCEDURE labTest.sp_GetAllAssets;
GO
CREATE PROCEDURE labTest.sp_GetAllAssets
AS
BEGIN
    SET NOCOUNT ON;
    SELECT 
        AssetId, AssetName, Category, PurchaseDate, Status, Price, CreatedAt, UpdatedAt
    FROM labTest.Assets
    ORDER BY AssetId DESC;
END;
GO

-- Procedure 2: sp_GetAssetById
IF OBJECT_ID('labTest.sp_GetAssetById', 'P') IS NOT NULL
    DROP PROCEDURE labTest.sp_GetAssetById;
GO
CREATE PROCEDURE labTest.sp_GetAssetById
    @AssetId INT
AS
BEGIN
    SET NOCOUNT ON;
    SELECT 
        AssetId, AssetName, Category, PurchaseDate, Status, Price, CreatedAt, UpdatedAt
    FROM labTest.Assets
    WHERE AssetId = @AssetId;
END;
GO

-- Procedure 3: sp_InsertAsset
IF OBJECT_ID('labTest.sp_InsertAsset', 'P') IS NOT NULL
    DROP PROCEDURE labTest.sp_InsertAsset;
GO
CREATE PROCEDURE labTest.sp_InsertAsset
    @AssetName    NVARCHAR(100),
    @Category     NVARCHAR(50),
    @PurchaseDate DATE,
    @Status       NVARCHAR(30),
    @Price        DECIMAL(18, 2)
AS
BEGIN
    SET NOCOUNT ON;
    INSERT INTO labTest.Assets (AssetName, Category, PurchaseDate, Status, Price, CreatedAt)
    VALUES (@AssetName, @Category, @PurchaseDate, @Status, @Price, GETDATE());

    SELECT CAST(SCOPE_IDENTITY() AS INT) AS NewAssetId;
END;
GO

-- Procedure 4: sp_UpdateAsset
IF OBJECT_ID('labTest.sp_UpdateAsset', 'P') IS NOT NULL
    DROP PROCEDURE labTest.sp_UpdateAsset;
GO
CREATE PROCEDURE labTest.sp_UpdateAsset
    @AssetId      INT,
    @AssetName    NVARCHAR(100),
    @Category     NVARCHAR(50),
    @PurchaseDate DATE,
    @Status       NVARCHAR(30),
    @Price        DECIMAL(18, 2)
AS
BEGIN
    SET NOCOUNT ON;
    UPDATE labTest.Assets
    SET 
        AssetName    = @AssetName,
        Category     = @Category,
        PurchaseDate = @PurchaseDate,
        Status       = @Status,
        Price        = @Price,
        UpdatedAt    = GETDATE()
    WHERE AssetId = @AssetId;

    SELECT @@ROWCOUNT AS RowsAffected;
END;
GO

-- Procedure 5: sp_DeleteAsset
IF OBJECT_ID('labTest.sp_DeleteAsset', 'P') IS NOT NULL
    DROP PROCEDURE labTest.sp_DeleteAsset;
GO
CREATE PROCEDURE labTest.sp_DeleteAsset
    @AssetId INT
AS
BEGIN
    SET NOCOUNT ON;
    DELETE FROM labTest.Assets
    WHERE AssetId = @AssetId;

    SELECT @@ROWCOUNT AS RowsAffected;
END;
GO

-- Procedure 6: sp_ValidateUser (for Auth / Login)
IF OBJECT_ID('labTest.sp_ValidateUser', 'P') IS NOT NULL
    DROP PROCEDURE labTest.sp_ValidateUser;
GO
CREATE PROCEDURE labTest.sp_ValidateUser
    @Username NVARCHAR(50),
    @Password NVARCHAR(256)
AS
BEGIN
    SET NOCOUNT ON;
    SELECT 
        UserId, Username, Role, IsActive
    FROM labTest.Users
    WHERE Username = @Username 
      AND PasswordHash = @Password
      AND IsActive = 1;
END;
GO

-- ============================================================================
-- ASSIGNMENT 2: Enterprise Hotel Booking & Reservation System
-- ============================================================================

-- 1. Users Table
IF OBJECT_ID('dbo.Users', 'U') IS NULL
BEGIN
    CREATE TABLE dbo.Users
    (
        UserId          INT IDENTITY(1,1) PRIMARY KEY,
        FullName        NVARCHAR(100)   NOT NULL,
        Email           NVARCHAR(150)   NOT NULL CONSTRAINT UQ_Users_Email UNIQUE,
        PasswordHash    NVARCHAR(256)   NOT NULL,
        PhoneNumber     NVARCHAR(20)    NOT NULL CONSTRAINT UQ_Users_PhoneNumber UNIQUE,
        Role            NVARCHAR(20)    NOT NULL DEFAULT 'Customer' CONSTRAINT CK_Users_Role CHECK (Role IN ('Admin', 'Customer')),
        IsActive        BIT             NOT NULL DEFAULT 1,
        CreatedAt       DATETIME2       NOT NULL DEFAULT GETDATE()
    );
END
GO

-- 2. RoomTypes Table
IF OBJECT_ID('dbo.RoomTypes', 'U') IS NULL
BEGIN
    CREATE TABLE dbo.RoomTypes
    (
        RoomTypeId        INT IDENTITY(1,1) PRIMARY KEY,
        TypeName          NVARCHAR(100)   NOT NULL CONSTRAINT UQ_RoomTypes_TypeName UNIQUE,
        Description       NVARCHAR(500)   NULL,
        BasePricePerNight DECIMAL(18, 2)  NOT NULL CONSTRAINT CK_RoomTypes_BasePrice CHECK (BasePricePerNight > 0),
        MaxOccupancy      INT             NOT NULL CONSTRAINT CK_RoomTypes_MaxOccupancy CHECK (MaxOccupancy >= 1),
        IsActive          BIT             NOT NULL DEFAULT 1,
        CreatedAt         DATETIME2       NOT NULL DEFAULT GETDATE()
    );
END
GO

-- 3. Rooms Table
IF OBJECT_ID('dbo.Rooms', 'U') IS NULL
BEGIN
    CREATE TABLE dbo.Rooms
    (
        RoomId          INT IDENTITY(1,1) PRIMARY KEY,
        RoomNumber      NVARCHAR(20)    NOT NULL CONSTRAINT UQ_Rooms_RoomNumber UNIQUE,
        RoomTypeId      INT             NOT NULL CONSTRAINT FK_Rooms_RoomTypes REFERENCES dbo.RoomTypes(RoomTypeId),
        FloorNumber     INT             NOT NULL,
        Status          NVARCHAR(30)    NOT NULL DEFAULT 'Available' CONSTRAINT CK_Rooms_Status CHECK (Status IN ('Available', 'Occupied', 'UnderMaintenance')),
        IsActive        BIT             NOT NULL DEFAULT 1,
        CreatedAt       DATETIME2       NOT NULL DEFAULT GETDATE()
    );
END
GO

-- 4. Amenities Table
IF OBJECT_ID('dbo.Amenities', 'U') IS NULL
BEGIN
    CREATE TABLE dbo.Amenities
    (
        AmenityId       INT IDENTITY(1,1) PRIMARY KEY,
        AmenityName     NVARCHAR(100)   NOT NULL CONSTRAINT UQ_Amenities_AmenityName UNIQUE,
        Description     NVARCHAR(500)   NULL,
        IconCode        NVARCHAR(50)    NULL,
        IsActive        BIT             NOT NULL DEFAULT 1,
        CreatedAt       DATETIME2       NOT NULL DEFAULT GETDATE()
    );
END
GO

-- 5. RoomAmenities (Junction Table)
IF OBJECT_ID('dbo.RoomAmenities', 'U') IS NULL
BEGIN
    CREATE TABLE dbo.RoomAmenities
    (
        RoomAmenityId   INT IDENTITY(1,1) PRIMARY KEY,
        RoomId          INT             NOT NULL CONSTRAINT FK_RoomAmenities_Rooms REFERENCES dbo.Rooms(RoomId) ON DELETE CASCADE,
        AmenityId       INT             NOT NULL CONSTRAINT FK_RoomAmenities_Amenities REFERENCES dbo.Amenities(AmenityId) ON DELETE CASCADE,
        CreatedAt       DATETIME2       NOT NULL DEFAULT GETDATE(),
        CONSTRAINT UQ_RoomAmenities_Room_Amenity UNIQUE (RoomId, AmenityId)
    );
END
GO

-- 6. Bookings Table
IF OBJECT_ID('dbo.Bookings', 'U') IS NULL
BEGIN
    CREATE TABLE dbo.Bookings
    (
        BookingId         INT IDENTITY(1,1) PRIMARY KEY,
        BookingReference  NVARCHAR(50)    NOT NULL CONSTRAINT UQ_Bookings_BookingReference UNIQUE,
        UserId            INT             NOT NULL CONSTRAINT FK_Bookings_Users REFERENCES dbo.Users(UserId),
        RoomId            INT             NOT NULL CONSTRAINT FK_Bookings_Rooms REFERENCES dbo.Rooms(RoomId),
        CheckInDate       DATE            NOT NULL,
        CheckOutDate      DATE            NOT NULL,
        NumberOfGuests    INT             NOT NULL CONSTRAINT CK_Bookings_Guests CHECK (NumberOfGuests >= 1),
        TotalAmount       DECIMAL(18, 2)  NOT NULL CONSTRAINT CK_Bookings_TotalAmount CHECK (TotalAmount >= 0),
        BookingStatus     NVARCHAR(20)    NOT NULL DEFAULT 'Confirmed' CONSTRAINT CK_Bookings_Status CHECK (BookingStatus IN ('Confirmed', 'Cancelled', 'Completed')),
        CreatedAt         DATETIME2       NOT NULL DEFAULT GETDATE(),
        CONSTRAINT CK_Bookings_Dates CHECK (CheckOutDate > CheckInDate)
    );
END
GO

-- 7. Cancellations Table
IF OBJECT_ID('dbo.Cancellations', 'U') IS NULL
BEGIN
    CREATE TABLE dbo.Cancellations
    (
        CancellationId      INT IDENTITY(1,1) PRIMARY KEY,
        BookingId           INT             NOT NULL CONSTRAINT UQ_Cancellations_BookingId UNIQUE CONSTRAINT FK_Cancellations_Bookings REFERENCES dbo.Bookings(BookingId),
        CancelledByUserId   INT             NOT NULL CONSTRAINT FK_Cancellations_Users REFERENCES dbo.Users(UserId),
        CancellationReason  NVARCHAR(500)   NULL,
        RefundAmount        DECIMAL(18, 2)  NOT NULL CONSTRAINT CK_Cancellations_Refund CHECK (RefundAmount >= 0),
        CancellationCharge  DECIMAL(18, 2)  NOT NULL CONSTRAINT CK_Cancellations_Charge CHECK (CancellationCharge >= 0),
        CancelledAt         DATETIME2       NOT NULL DEFAULT GETDATE()
    );
END
GO

-- 8. AuditLogs Table
IF OBJECT_ID('dbo.AuditLogs', 'U') IS NULL
BEGIN
    CREATE TABLE dbo.AuditLogs
    (
        AuditLogId      INT IDENTITY(1,1) PRIMARY KEY,
        EntityName      NVARCHAR(50)    NOT NULL,
        EntityId        INT             NULL,
        Action          NVARCHAR(50)    NOT NULL,
        UserId          INT             NULL,
        OldValues       NVARCHAR(MAX)   NULL,
        NewValues       NVARCHAR(MAX)   NULL,
        Details         NVARCHAR(500)   NULL,
        Timestamp       DATETIME2       NOT NULL DEFAULT GETDATE()
    );
END
GO

-- ============================================================================
-- INDEXES FOR SEARCH OPTIMIZATION & PERFORMANCE
-- ============================================================================

IF NOT EXISTS (SELECT 1 FROM sys.indexes WHERE name = 'IX_Rooms_RoomTypeId_Status_IsActive' AND object_id = OBJECT_ID('dbo.Rooms'))
BEGIN
    CREATE NONCLUSTERED INDEX IX_Rooms_RoomTypeId_Status_IsActive 
    ON dbo.Rooms (RoomTypeId, Status, IsActive)
    INCLUDE (RoomNumber, FloorNumber);
END
GO

IF NOT EXISTS (SELECT 1 FROM sys.indexes WHERE name = 'IX_Bookings_RoomId_Dates_Status' AND object_id = OBJECT_ID('dbo.Bookings'))
BEGIN
    CREATE NONCLUSTERED INDEX IX_Bookings_RoomId_Dates_Status 
    ON dbo.Bookings (RoomId, CheckInDate, CheckOutDate, BookingStatus);
END
GO

IF NOT EXISTS (SELECT 1 FROM sys.indexes WHERE name = 'IX_Bookings_UserId' AND object_id = OBJECT_ID('dbo.Bookings'))
BEGIN
    CREATE NONCLUSTERED INDEX IX_Bookings_UserId 
    ON dbo.Bookings (UserId, BookingStatus);
END
GO

IF NOT EXISTS (SELECT 1 FROM sys.indexes WHERE name = 'IX_Bookings_BookingReference' AND object_id = OBJECT_ID('dbo.Bookings'))
BEGIN
    CREATE NONCLUSTERED INDEX IX_Bookings_BookingReference 
    ON dbo.Bookings (BookingReference);
END
GO

IF NOT EXISTS (SELECT 1 FROM sys.indexes WHERE name = 'IX_RoomAmenities_RoomId' AND object_id = OBJECT_ID('dbo.RoomAmenities'))
BEGIN
    CREATE NONCLUSTERED INDEX IX_RoomAmenities_RoomId 
    ON dbo.RoomAmenities (RoomId, AmenityId);
END
GO

IF NOT EXISTS (SELECT 1 FROM sys.indexes WHERE name = 'IX_RoomAmenities_AmenityId' AND object_id = OBJECT_ID('dbo.RoomAmenities'))
BEGIN
    CREATE NONCLUSTERED INDEX IX_RoomAmenities_AmenityId 
    ON dbo.RoomAmenities (AmenityId, RoomId);
END
GO

IF NOT EXISTS (SELECT 1 FROM sys.indexes WHERE name = 'IX_Users_Email' AND object_id = OBJECT_ID('dbo.Users'))
BEGIN
    CREATE NONCLUSTERED INDEX IX_Users_Email 
    ON dbo.Users (Email);
END
GO

-- ============================================================================
-- SEED DATA (Assignment 2)
-- ============================================================================

-- Seed Users
IF NOT EXISTS (SELECT 1 FROM dbo.Users WHERE Email = 'admin@hotel.com')
BEGIN
    INSERT INTO dbo.Users (FullName, Email, PasswordHash, PhoneNumber, Role, IsActive)
    VALUES ('System Administrator', 'admin@hotel.com', 'Admin@12345', '+19876543210', 'Admin', 1);
END

IF NOT EXISTS (SELECT 1 FROM dbo.Users WHERE Email = 'customer@hotel.com')
BEGIN
    INSERT INTO dbo.Users (FullName, Email, PasswordHash, PhoneNumber, Role, IsActive)
    VALUES ('John Doe', 'customer@hotel.com', 'Customer@12345', '+19876543211', 'Customer', 1);
END
GO

-- Seed RoomTypes
IF NOT EXISTS (SELECT 1 FROM dbo.RoomTypes WHERE TypeName = 'Standard')
BEGIN
    INSERT INTO dbo.RoomTypes (TypeName, Description, BasePricePerNight, MaxOccupancy, IsActive)
    VALUES 
        ('Standard', 'Comfortable single/double occupancy room with essential amenities', 80.00, 2, 1),
        ('Deluxe', 'Spacious room with king bed, work desk, and scenic city views', 150.00, 3, 1),
        ('Executive Suite', 'Luxury suite featuring separate living area, panoramic view, and premium perks', 280.00, 4, 1);
END
GO

-- Seed Rooms
IF NOT EXISTS (SELECT 1 FROM dbo.Rooms WHERE RoomNumber = '101')
BEGIN
    DECLARE @StandardId INT = (SELECT RoomTypeId FROM dbo.RoomTypes WHERE TypeName = 'Standard');
    DECLARE @DeluxeId INT = (SELECT RoomTypeId FROM dbo.RoomTypes WHERE TypeName = 'Deluxe');
    DECLARE @SuiteId INT = (SELECT RoomTypeId FROM dbo.RoomTypes WHERE TypeName = 'Executive Suite');

    INSERT INTO dbo.Rooms (RoomNumber, RoomTypeId, FloorNumber, Status, IsActive)
    VALUES
        ('101', @StandardId, 1, 'Available', 1),
        ('102', @StandardId, 1, 'Available', 1),
        ('201', @DeluxeId, 2, 'Available', 1),
        ('202', @DeluxeId, 2, 'Available', 1),
        ('301', @SuiteId, 3, 'Available', 1),
        ('302', @SuiteId, 3, 'UnderMaintenance', 1);
END
GO

-- Seed Amenities
IF NOT EXISTS (SELECT 1 FROM dbo.Amenities WHERE AmenityName = 'High-Speed Wi-Fi')
BEGIN
    INSERT INTO dbo.Amenities (AmenityName, Description, IconCode, IsActive)
    VALUES
        ('High-Speed Wi-Fi', 'Complimentary fiber optic wireless internet', 'wifi', 1),
        ('Air Conditioning', 'Climate control heating and cooling', 'ac', 1),
        ('Mini Bar', 'Stocked mini refrigerator with refreshments and snacks', 'bar', 1),
        ('Ocean View', 'Balcony facing the ocean with scenic morning view', 'ocean', 1),
        ('Jacuzzi', 'Private in-room whirlpool hydrotherapy bath', 'jacuzzi', 1),
        ('Breakfast Included', 'Daily complimentary buffet breakfast', 'breakfast', 1);
END
GO

-- Seed RoomAmenities
IF NOT EXISTS (SELECT 1 FROM dbo.RoomAmenities)
BEGIN
    DECLARE @Room101 INT = (SELECT RoomId FROM dbo.Rooms WHERE RoomNumber = '101');
    DECLARE @Room201 INT = (SELECT RoomId FROM dbo.Rooms WHERE RoomNumber = '201');
    DECLARE @Room301 INT = (SELECT RoomId FROM dbo.Rooms WHERE RoomNumber = '301');

    DECLARE @Wifi INT = (SELECT AmenityId FROM dbo.Amenities WHERE AmenityName = 'High-Speed Wi-Fi');
    DECLARE @AC INT = (SELECT AmenityId FROM dbo.Amenities WHERE AmenityName = 'Air Conditioning');
    DECLARE @MiniBar INT = (SELECT AmenityId FROM dbo.Amenities WHERE AmenityName = 'Mini Bar');
    DECLARE @OceanView INT = (SELECT AmenityId FROM dbo.Amenities WHERE AmenityName = 'Ocean View');
    DECLARE @Jacuzzi INT = (SELECT AmenityId FROM dbo.Amenities WHERE AmenityName = 'Jacuzzi');
    DECLARE @Breakfast INT = (SELECT AmenityId FROM dbo.Amenities WHERE AmenityName = 'Breakfast Included');

    IF @Room101 IS NOT NULL AND @Wifi IS NOT NULL AND @AC IS NOT NULL
        INSERT INTO dbo.RoomAmenities (RoomId, AmenityId) VALUES (@Room101, @Wifi), (@Room101, @AC);

    IF @Room201 IS NOT NULL AND @Wifi IS NOT NULL AND @AC IS NOT NULL AND @MiniBar IS NOT NULL AND @Breakfast IS NOT NULL
        INSERT INTO dbo.RoomAmenities (RoomId, AmenityId) VALUES (@Room201, @Wifi), (@Room201, @AC), (@Room201, @MiniBar), (@Room201, @Breakfast);

    IF @Room301 IS NOT NULL AND @Wifi IS NOT NULL AND @AC IS NOT NULL AND @MiniBar IS NOT NULL AND @OceanView IS NOT NULL AND @Jacuzzi IS NOT NULL AND @Breakfast IS NOT NULL
        INSERT INTO dbo.RoomAmenities (RoomId, AmenityId) VALUES (@Room301, @Wifi), (@Room301, @AC), (@Room301, @MiniBar), (@Room301, @OceanView), (@Room301, @Jacuzzi), (@Room301, @Breakfast);
END
GO

-- ============================================================================
-- STORED PROCEDURES (Assignment 2)
-- ============================================================================

-- ----------------------------------------------------------------------------
-- MODULE 3: USER MODULE PROCEDURES
-- ----------------------------------------------------------------------------

IF OBJECT_ID('dbo.usp_User_Register', 'P') IS NOT NULL
    DROP PROCEDURE dbo.usp_User_Register;
GO
CREATE PROCEDURE dbo.usp_User_Register
    @FullName     NVARCHAR(100),
    @Email        NVARCHAR(150),
    @PasswordHash NVARCHAR(256),
    @PhoneNumber  NVARCHAR(20),
    @Role         NVARCHAR(20) = 'Customer'
AS
BEGIN
    SET NOCOUNT ON;

    BEGIN TRY
        BEGIN TRANSACTION;

        IF EXISTS (SELECT 1 FROM dbo.Users WHERE Email = @Email)
        BEGIN
            THROW 50001, 'Email address is already registered.', 1;
        END

        IF EXISTS (SELECT 1 FROM dbo.Users WHERE PhoneNumber = @PhoneNumber)
        BEGIN
            THROW 50002, 'Phone number is already registered.', 1;
        END

        IF @Role NOT IN ('Admin', 'Customer')
            SET @Role = 'Customer';

        INSERT INTO dbo.Users (FullName, Email, PasswordHash, PhoneNumber, Role, IsActive, CreatedAt)
        VALUES (@FullName, @Email, @PasswordHash, @PhoneNumber, @Role, 1, GETDATE());

        DECLARE @NewUserId INT = SCOPE_IDENTITY();

        INSERT INTO dbo.AuditLogs (EntityName, EntityId, Action, UserId, Details, Timestamp)
        VALUES ('Users', @NewUserId, 'INSERT', @NewUserId, 'User registered successfully with role ' + @Role, GETDATE());

        COMMIT TRANSACTION;

        SELECT 
            UserId, FullName, Email, PhoneNumber, Role, IsActive, CreatedAt
        FROM dbo.Users
        WHERE UserId = @NewUserId;
    END TRY
    BEGIN CATCH
        IF @@TRANCOUNT > 0
            ROLLBACK TRANSACTION;
        THROW;
    END CATCH;
END;
GO

IF OBJECT_ID('dbo.usp_User_Login', 'P') IS NOT NULL
    DROP PROCEDURE dbo.usp_User_Login;
GO
CREATE PROCEDURE dbo.usp_User_Login
    @Email NVARCHAR(150)
AS
BEGIN
    SET NOCOUNT ON;

    SELECT 
        UserId, FullName, Email, PasswordHash, PhoneNumber, Role, IsActive, CreatedAt
    FROM dbo.Users
    WHERE Email = @Email AND IsActive = 1;
END;
GO

IF OBJECT_ID('dbo.usp_User_GetById', 'P') IS NOT NULL
    DROP PROCEDURE dbo.usp_User_GetById;
GO
CREATE PROCEDURE dbo.usp_User_GetById
    @UserId INT
AS
BEGIN
    SET NOCOUNT ON;

    SELECT 
        UserId, FullName, Email, PhoneNumber, Role, IsActive, CreatedAt
    FROM dbo.Users
    WHERE UserId = @UserId;
END;
GO

IF OBJECT_ID('dbo.usp_User_UpdateProfile', 'P') IS NOT NULL
    DROP PROCEDURE dbo.usp_User_UpdateProfile;
GO
CREATE PROCEDURE dbo.usp_User_UpdateProfile
    @UserId      INT,
    @FullName    NVARCHAR(100),
    @PhoneNumber NVARCHAR(20)
AS
BEGIN
    SET NOCOUNT ON;

    BEGIN TRY
        BEGIN TRANSACTION;

        IF NOT EXISTS (SELECT 1 FROM dbo.Users WHERE UserId = @UserId)
        BEGIN
            THROW 50003, 'User not found.', 1;
        END

        IF EXISTS (SELECT 1 FROM dbo.Users WHERE PhoneNumber = @PhoneNumber AND UserId <> @UserId)
        BEGIN
            THROW 50002, 'Phone number is already in use by another user.', 1;
        END

        UPDATE dbo.Users
        SET FullName = @FullName,
            PhoneNumber = @PhoneNumber
        WHERE UserId = @UserId;

        INSERT INTO dbo.AuditLogs (EntityName, EntityId, Action, UserId, Details, Timestamp)
        VALUES ('Users', @UserId, 'UPDATE', @UserId, 'Profile updated', GETDATE());

        COMMIT TRANSACTION;

        SELECT 
            UserId, FullName, Email, PhoneNumber, Role, IsActive, CreatedAt
        FROM dbo.Users
        WHERE UserId = @UserId;
    END TRY
    BEGIN CATCH
        IF @@TRANCOUNT > 0
            ROLLBACK TRANSACTION;
        THROW;
    END CATCH;
END;
GO

IF OBJECT_ID('dbo.usp_User_ToggleStatus', 'P') IS NOT NULL
    DROP PROCEDURE dbo.usp_User_ToggleStatus;
GO
CREATE PROCEDURE dbo.usp_User_ToggleStatus
    @UserId   INT,
    @IsActive BIT
AS
BEGIN
    SET NOCOUNT ON;

    BEGIN TRY
        BEGIN TRANSACTION;

        IF NOT EXISTS (SELECT 1 FROM dbo.Users WHERE UserId = @UserId)
        BEGIN
            THROW 50003, 'User not found.', 1;
        END

        UPDATE dbo.Users
        SET IsActive = @IsActive
        WHERE UserId = @UserId;

        INSERT INTO dbo.AuditLogs (EntityName, EntityId, Action, UserId, Details, Timestamp)
        VALUES ('Users', @UserId, 'STATUS_CHANGE', @UserId, 'User active status changed to ' + CAST(@IsActive AS NVARCHAR(1)), GETDATE());

        COMMIT TRANSACTION;

        SELECT @@ROWCOUNT AS RowsAffected;
    END TRY
    BEGIN CATCH
        IF @@TRANCOUNT > 0
            ROLLBACK TRANSACTION;
        THROW;
    END CATCH;
END;
GO

-- ----------------------------------------------------------------------------
-- MODULE 4: ROOM TYPE MODULE PROCEDURES
-- ----------------------------------------------------------------------------

IF OBJECT_ID('dbo.usp_RoomType_Create', 'P') IS NOT NULL
    DROP PROCEDURE dbo.usp_RoomType_Create;
GO
CREATE PROCEDURE dbo.usp_RoomType_Create
    @TypeName          NVARCHAR(100),
    @Description       NVARCHAR(500) = NULL,
    @BasePricePerNight DECIMAL(18, 2),
    @MaxOccupancy      INT
AS
BEGIN
    SET NOCOUNT ON;

    BEGIN TRY
        BEGIN TRANSACTION;

        IF @BasePricePerNight <= 0
            THROW 50011, 'BasePricePerNight must be greater than 0.', 1;

        IF @MaxOccupancy < 1
            THROW 50012, 'MaxOccupancy must be at least 1.', 1;

        IF EXISTS (SELECT 1 FROM dbo.RoomTypes WHERE LOWER(TypeName) = LOWER(@TypeName))
            THROW 50013, 'RoomType name already exists.', 1;

        INSERT INTO dbo.RoomTypes (TypeName, Description, BasePricePerNight, MaxOccupancy, IsActive, CreatedAt)
        VALUES (@TypeName, @Description, @BasePricePerNight, @MaxOccupancy, 1, GETDATE());

        DECLARE @NewRoomTypeId INT = SCOPE_IDENTITY();

        INSERT INTO dbo.AuditLogs (EntityName, EntityId, Action, Details, Timestamp)
        VALUES ('RoomTypes', @NewRoomTypeId, 'INSERT', 'Created room type ' + @TypeName, GETDATE());

        COMMIT TRANSACTION;

        SELECT 
            RoomTypeId, TypeName, Description, BasePricePerNight, MaxOccupancy, IsActive, CreatedAt
        FROM dbo.RoomTypes
        WHERE RoomTypeId = @NewRoomTypeId;
    END TRY
    BEGIN CATCH
        IF @@TRANCOUNT > 0
            ROLLBACK TRANSACTION;
        THROW;
    END CATCH;
END;
GO

IF OBJECT_ID('dbo.usp_RoomType_GetAll', 'P') IS NOT NULL
    DROP PROCEDURE dbo.usp_RoomType_GetAll;
GO
CREATE PROCEDURE dbo.usp_RoomType_GetAll
    @IncludeInactive BIT = 0
AS
BEGIN
    SET NOCOUNT ON;

    SELECT 
        RoomTypeId, TypeName, Description, BasePricePerNight, MaxOccupancy, IsActive, CreatedAt
    FROM dbo.RoomTypes
    WHERE (@IncludeInactive = 1 OR IsActive = 1)
    ORDER BY RoomTypeId;
END;
GO

IF OBJECT_ID('dbo.usp_RoomType_GetById', 'P') IS NOT NULL
    DROP PROCEDURE dbo.usp_RoomType_GetById;
GO
CREATE PROCEDURE dbo.usp_RoomType_GetById
    @RoomTypeId INT
AS
BEGIN
    SET NOCOUNT ON;

    SELECT 
        RoomTypeId, TypeName, Description, BasePricePerNight, MaxOccupancy, IsActive, CreatedAt
    FROM dbo.RoomTypes
    WHERE RoomTypeId = @RoomTypeId;
END;
GO

IF OBJECT_ID('dbo.usp_RoomType_Update', 'P') IS NOT NULL
    DROP PROCEDURE dbo.usp_RoomType_Update;
GO
CREATE PROCEDURE dbo.usp_RoomType_Update
    @RoomTypeId        INT,
    @TypeName          NVARCHAR(100),
    @Description       NVARCHAR(500) = NULL,
    @BasePricePerNight DECIMAL(18, 2),
    @MaxOccupancy      INT,
    @IsActive          BIT = 1
AS
BEGIN
    SET NOCOUNT ON;

    BEGIN TRY
        BEGIN TRANSACTION;

        IF NOT EXISTS (SELECT 1 FROM dbo.RoomTypes WHERE RoomTypeId = @RoomTypeId)
            THROW 50010, 'RoomType not found.', 1;

        IF @BasePricePerNight <= 0
            THROW 50011, 'BasePricePerNight must be greater than 0.', 1;

        IF @MaxOccupancy < 1
            THROW 50012, 'MaxOccupancy must be at least 1.', 1;

        IF EXISTS (SELECT 1 FROM dbo.RoomTypes WHERE LOWER(TypeName) = LOWER(@TypeName) AND RoomTypeId <> @RoomTypeId)
            THROW 50013, 'Another RoomType with this name already exists.', 1;

        UPDATE dbo.RoomTypes
        SET TypeName = @TypeName,
            Description = @Description,
            BasePricePerNight = @BasePricePerNight,
            MaxOccupancy = @MaxOccupancy,
            IsActive = @IsActive
        WHERE RoomTypeId = @RoomTypeId;

        INSERT INTO dbo.AuditLogs (EntityName, EntityId, Action, Details, Timestamp)
        VALUES ('RoomTypes', @RoomTypeId, 'UPDATE', 'Updated room type ' + @TypeName, GETDATE());

        COMMIT TRANSACTION;

        SELECT 
            RoomTypeId, TypeName, Description, BasePricePerNight, MaxOccupancy, IsActive, CreatedAt
        FROM dbo.RoomTypes
        WHERE RoomTypeId = @RoomTypeId;
    END TRY
    BEGIN CATCH
        IF @@TRANCOUNT > 0
            ROLLBACK TRANSACTION;
        THROW;
    END CATCH;
END;
GO

IF OBJECT_ID('dbo.usp_RoomType_SoftDelete', 'P') IS NOT NULL
    DROP PROCEDURE dbo.usp_RoomType_SoftDelete;
GO
CREATE PROCEDURE dbo.usp_RoomType_SoftDelete
    @RoomTypeId INT
AS
BEGIN
    SET NOCOUNT ON;

    BEGIN TRY
        BEGIN TRANSACTION;

        IF NOT EXISTS (SELECT 1 FROM dbo.RoomTypes WHERE RoomTypeId = @RoomTypeId)
            THROW 50010, 'RoomType not found.', 1;

        -- Soft-delete only if no active future bookings exist for rooms under this type
        IF EXISTS (
            SELECT 1 
            FROM dbo.Bookings b
            JOIN dbo.Rooms r ON b.RoomId = r.RoomId
            WHERE r.RoomTypeId = @RoomTypeId
              AND b.BookingStatus = 'Confirmed'
              AND b.CheckOutDate >= CAST(GETDATE() AS DATE)
        )
        BEGIN
            THROW 50014, 'Cannot deactivate Room Type. Active or future bookings exist for rooms of this type.', 1;
        END

        UPDATE dbo.RoomTypes
        SET IsActive = 0
        WHERE RoomTypeId = @RoomTypeId;

        INSERT INTO dbo.AuditLogs (EntityName, EntityId, Action, Details, Timestamp)
        VALUES ('RoomTypes', @RoomTypeId, 'DEACTIVATE', 'Deactivated room type', GETDATE());

        COMMIT TRANSACTION;

        SELECT @@ROWCOUNT AS RowsAffected;
    END TRY
    BEGIN CATCH
        IF @@TRANCOUNT > 0
            ROLLBACK TRANSACTION;
        THROW;
    END CATCH;
END;
GO

-- ----------------------------------------------------------------------------
-- MODULE 5: ROOM MODULE PROCEDURES
-- ----------------------------------------------------------------------------

IF OBJECT_ID('dbo.usp_Room_Create', 'P') IS NOT NULL
    DROP PROCEDURE dbo.usp_Room_Create;
GO
CREATE PROCEDURE dbo.usp_Room_Create
    @RoomNumber  NVARCHAR(20),
    @RoomTypeId  INT,
    @FloorNumber INT,
    @Status      NVARCHAR(30) = 'Available'
AS
BEGIN
    SET NOCOUNT ON;

    BEGIN TRY
        BEGIN TRANSACTION;

        IF EXISTS (SELECT 1 FROM dbo.Rooms WHERE RoomNumber = @RoomNumber)
            THROW 50020, 'RoomNumber already exists.', 1;

        IF NOT EXISTS (SELECT 1 FROM dbo.RoomTypes WHERE RoomTypeId = @RoomTypeId AND IsActive = 1)
            THROW 50021, 'Valid active RoomType is required.', 1;

        IF @Status NOT IN ('Available', 'Occupied', 'UnderMaintenance')
            SET @Status = 'Available';

        INSERT INTO dbo.Rooms (RoomNumber, RoomTypeId, FloorNumber, Status, IsActive, CreatedAt)
        VALUES (@RoomNumber, @RoomTypeId, @FloorNumber, @Status, 1, GETDATE());

        DECLARE @NewRoomId INT = SCOPE_IDENTITY();

        INSERT INTO dbo.AuditLogs (EntityName, EntityId, Action, Details, Timestamp)
        VALUES ('Rooms', @NewRoomId, 'INSERT', 'Created room ' + @RoomNumber, GETDATE());

        COMMIT TRANSACTION;

        SELECT 
            r.RoomId, r.RoomNumber, r.FloorNumber, r.Status, r.IsActive, r.CreatedAt,
            rt.RoomTypeId, rt.TypeName AS RoomTypeName, rt.Description AS RoomTypeDescription,
            rt.BasePricePerNight, rt.MaxOccupancy
        FROM dbo.Rooms r
        JOIN dbo.RoomTypes rt ON r.RoomTypeId = rt.RoomTypeId
        WHERE r.RoomId = @NewRoomId;
    END TRY
    BEGIN CATCH
        IF @@TRANCOUNT > 0
            ROLLBACK TRANSACTION;
        THROW;
    END CATCH;
END;
GO

IF OBJECT_ID('dbo.usp_Room_GetAll', 'P') IS NOT NULL
    DROP PROCEDURE dbo.usp_Room_GetAll;
GO
CREATE PROCEDURE dbo.usp_Room_GetAll
    @IncludeInactive BIT = 0
AS
BEGIN
    SET NOCOUNT ON;

    SELECT 
        r.RoomId, r.RoomNumber, r.FloorNumber, r.Status, r.IsActive, r.CreatedAt,
        rt.RoomTypeId, rt.TypeName AS RoomTypeName, rt.Description AS RoomTypeDescription,
        rt.BasePricePerNight, rt.MaxOccupancy
    FROM dbo.Rooms r
    JOIN dbo.RoomTypes rt ON r.RoomTypeId = rt.RoomTypeId
    WHERE (@IncludeInactive = 1 OR r.IsActive = 1)
    ORDER BY r.RoomNumber;
END;
GO

IF OBJECT_ID('dbo.usp_Room_GetById', 'P') IS NOT NULL
    DROP PROCEDURE dbo.usp_Room_GetById;
GO
CREATE PROCEDURE dbo.usp_Room_GetById
    @RoomId INT
AS
BEGIN
    SET NOCOUNT ON;

    -- Result Set 1: Room Details
    SELECT 
        r.RoomId, r.RoomNumber, r.FloorNumber, r.Status, r.IsActive, r.CreatedAt,
        rt.RoomTypeId, rt.TypeName AS RoomTypeName, rt.Description AS RoomTypeDescription,
        rt.BasePricePerNight, rt.MaxOccupancy
    FROM dbo.Rooms r
    JOIN dbo.RoomTypes rt ON r.RoomTypeId = rt.RoomTypeId
    WHERE r.RoomId = @RoomId;

    -- Result Set 2: Mapped Amenities
    SELECT 
        a.AmenityId, a.AmenityName, a.Description, a.IconCode, a.IsActive
    FROM dbo.RoomAmenities ra
    JOIN dbo.Amenities a ON ra.AmenityId = a.AmenityId
    WHERE ra.RoomId = @RoomId AND a.IsActive = 1
    ORDER BY a.AmenityName;
END;
GO

IF OBJECT_ID('dbo.usp_Room_Update', 'P') IS NOT NULL
    DROP PROCEDURE dbo.usp_Room_Update;
GO
CREATE PROCEDURE dbo.usp_Room_Update
    @RoomId      INT,
    @RoomNumber  NVARCHAR(20),
    @RoomTypeId  INT,
    @FloorNumber INT,
    @Status      NVARCHAR(30),
    @IsActive    BIT
AS
BEGIN
    SET NOCOUNT ON;

    BEGIN TRY
        BEGIN TRANSACTION;

        IF NOT EXISTS (SELECT 1 FROM dbo.Rooms WHERE RoomId = @RoomId)
            THROW 50022, 'Room not found.', 1;

        IF EXISTS (SELECT 1 FROM dbo.Rooms WHERE RoomNumber = @RoomNumber AND RoomId <> @RoomId)
            THROW 50020, 'RoomNumber is already in use by another room.', 1;

        IF NOT EXISTS (SELECT 1 FROM dbo.RoomTypes WHERE RoomTypeId = @RoomTypeId)
            THROW 50021, 'RoomType does not exist.', 1;

        IF @Status NOT IN ('Available', 'Occupied', 'UnderMaintenance')
            THROW 50023, 'Invalid status.', 1;

        UPDATE dbo.Rooms
        SET RoomNumber = @RoomNumber,
            RoomTypeId = @RoomTypeId,
            FloorNumber = @FloorNumber,
            Status = @Status,
            IsActive = @IsActive
        WHERE RoomId = @RoomId;

        INSERT INTO dbo.AuditLogs (EntityName, EntityId, Action, Details, Timestamp)
        VALUES ('Rooms', @RoomId, 'UPDATE', 'Updated room details for ' + @RoomNumber, GETDATE());

        COMMIT TRANSACTION;

        SELECT 
            r.RoomId, r.RoomNumber, r.FloorNumber, r.Status, r.IsActive, r.CreatedAt,
            rt.RoomTypeId, rt.TypeName AS RoomTypeName, rt.Description AS RoomTypeDescription,
            rt.BasePricePerNight, rt.MaxOccupancy
        FROM dbo.Rooms r
        JOIN dbo.RoomTypes rt ON r.RoomTypeId = rt.RoomTypeId
        WHERE r.RoomId = @RoomId;
    END TRY
    BEGIN CATCH
        IF @@TRANCOUNT > 0
            ROLLBACK TRANSACTION;
        THROW;
    END CATCH;
END;
GO

IF OBJECT_ID('dbo.usp_Room_UpdateStatus', 'P') IS NOT NULL
    DROP PROCEDURE dbo.usp_Room_UpdateStatus;
GO
CREATE PROCEDURE dbo.usp_Room_UpdateStatus
    @RoomId INT,
    @Status NVARCHAR(30)
AS
BEGIN
    SET NOCOUNT ON;

    BEGIN TRY
        BEGIN TRANSACTION;

        IF NOT EXISTS (SELECT 1 FROM dbo.Rooms WHERE RoomId = @RoomId)
            THROW 50022, 'Room not found.', 1;

        IF @Status NOT IN ('Available', 'Occupied', 'UnderMaintenance')
            THROW 50023, 'Invalid status. Must be Available, Occupied, or UnderMaintenance.', 1;

        IF @Status = 'UnderMaintenance' AND EXISTS (
            SELECT 1 FROM dbo.Bookings
            WHERE RoomId = @RoomId
              AND BookingStatus = 'Confirmed'
              AND CheckInDate <= CAST(GETDATE() AS DATE)
              AND CheckOutDate > CAST(GETDATE() AS DATE)
        )
        BEGIN
            THROW 50024, 'Cannot put room under maintenance while an active booking is in progress.', 1;
        END

        UPDATE dbo.Rooms
        SET Status = @Status
        WHERE RoomId = @RoomId;

        INSERT INTO dbo.AuditLogs (EntityName, EntityId, Action, Details, Timestamp)
        VALUES ('Rooms', @RoomId, 'STATUS_CHANGE', 'Status changed to ' + @Status, GETDATE());

        COMMIT TRANSACTION;

        SELECT @@ROWCOUNT AS RowsAffected;
    END TRY
    BEGIN CATCH
        IF @@TRANCOUNT > 0
            ROLLBACK TRANSACTION;
        THROW;
    END CATCH;
END;
GO

-- ----------------------------------------------------------------------------
-- MODULE 6: AMENITIES MODULE PROCEDURES
-- ----------------------------------------------------------------------------

IF OBJECT_ID('dbo.usp_Amenity_Create', 'P') IS NOT NULL
    DROP PROCEDURE dbo.usp_Amenity_Create;
GO
CREATE PROCEDURE dbo.usp_Amenity_Create
    @AmenityName NVARCHAR(100),
    @Description NVARCHAR(500) = NULL,
    @IconCode    NVARCHAR(50)  = NULL
AS
BEGIN
    SET NOCOUNT ON;

    BEGIN TRY
        BEGIN TRANSACTION;

        IF EXISTS (SELECT 1 FROM dbo.Amenities WHERE LOWER(AmenityName) = LOWER(@AmenityName))
            THROW 50030, 'Amenity with this name already exists.', 1;

        INSERT INTO dbo.Amenities (AmenityName, Description, IconCode, IsActive, CreatedAt)
        VALUES (@AmenityName, @Description, @IconCode, 1, GETDATE());

        DECLARE @NewAmenityId INT = SCOPE_IDENTITY();

        INSERT INTO dbo.AuditLogs (EntityName, EntityId, Action, Details, Timestamp)
        VALUES ('Amenities', @NewAmenityId, 'INSERT', 'Created amenity ' + @AmenityName, GETDATE());

        COMMIT TRANSACTION;

        SELECT 
            AmenityId, AmenityName, Description, IconCode, IsActive, CreatedAt
        FROM dbo.Amenities
        WHERE AmenityId = @NewAmenityId;
    END TRY
    BEGIN CATCH
        IF @@TRANCOUNT > 0
            ROLLBACK TRANSACTION;
        THROW;
    END CATCH;
END;
GO

IF OBJECT_ID('dbo.usp_Amenity_GetAll', 'P') IS NOT NULL
    DROP PROCEDURE dbo.usp_Amenity_GetAll;
GO
CREATE PROCEDURE dbo.usp_Amenity_GetAll
    @IncludeInactive BIT = 0
AS
BEGIN
    SET NOCOUNT ON;

    SELECT 
        AmenityId, AmenityName, Description, IconCode, IsActive, CreatedAt
    FROM dbo.Amenities
    WHERE (@IncludeInactive = 1 OR IsActive = 1)
    ORDER BY AmenityName;
END;
GO

IF OBJECT_ID('dbo.usp_Amenity_Update', 'P') IS NOT NULL
    DROP PROCEDURE dbo.usp_Amenity_Update;
GO
CREATE PROCEDURE dbo.usp_Amenity_Update
    @AmenityId   INT,
    @AmenityName NVARCHAR(100),
    @Description NVARCHAR(500) = NULL,
    @IconCode    NVARCHAR(50)  = NULL,
    @IsActive    BIT           = 1
AS
BEGIN
    SET NOCOUNT ON;

    BEGIN TRY
        BEGIN TRANSACTION;

        IF NOT EXISTS (SELECT 1 FROM dbo.Amenities WHERE AmenityId = @AmenityId)
            THROW 50031, 'Amenity not found.', 1;

        IF EXISTS (SELECT 1 FROM dbo.Amenities WHERE LOWER(AmenityName) = LOWER(@AmenityName) AND AmenityId <> @AmenityId)
            THROW 50030, 'Another amenity with this name already exists.', 1;

        UPDATE dbo.Amenities
        SET AmenityName = @AmenityName,
            Description = @Description,
            IconCode = @IconCode,
            IsActive = @IsActive
        WHERE AmenityId = @AmenityId;

        INSERT INTO dbo.AuditLogs (EntityName, EntityId, Action, Details, Timestamp)
        VALUES ('Amenities', @AmenityId, 'UPDATE', 'Updated amenity ' + @AmenityName, GETDATE());

        COMMIT TRANSACTION;

        SELECT 
            AmenityId, AmenityName, Description, IconCode, IsActive, CreatedAt
        FROM dbo.Amenities
        WHERE AmenityId = @AmenityId;
    END TRY
    BEGIN CATCH
        IF @@TRANCOUNT > 0
            ROLLBACK TRANSACTION;
        THROW;
    END CATCH;
END;
GO

IF OBJECT_ID('dbo.usp_Amenity_SoftDelete', 'P') IS NOT NULL
    DROP PROCEDURE dbo.usp_Amenity_SoftDelete;
GO
CREATE PROCEDURE dbo.usp_Amenity_SoftDelete
    @AmenityId INT
AS
BEGIN
    SET NOCOUNT ON;

    BEGIN TRY
        BEGIN TRANSACTION;

        IF NOT EXISTS (SELECT 1 FROM dbo.Amenities WHERE AmenityId = @AmenityId)
            THROW 50031, 'Amenity not found.', 1;

        UPDATE dbo.Amenities
        SET IsActive = 0
        WHERE AmenityId = @AmenityId;

        INSERT INTO dbo.AuditLogs (EntityName, EntityId, Action, Details, Timestamp)
        VALUES ('Amenities', @AmenityId, 'DEACTIVATE', 'Deactivated amenity', GETDATE());

        COMMIT TRANSACTION;

        SELECT @@ROWCOUNT AS RowsAffected;
    END TRY
    BEGIN CATCH
        IF @@TRANCOUNT > 0
            ROLLBACK TRANSACTION;
        THROW;
    END CATCH;
END;
GO

-- ----------------------------------------------------------------------------
-- MODULE 7: ROOM AMENITIES MODULE PROCEDURES
-- ----------------------------------------------------------------------------

IF OBJECT_ID('dbo.usp_RoomAmenities_Assign', 'P') IS NOT NULL
    DROP PROCEDURE dbo.usp_RoomAmenities_Assign;
GO
CREATE PROCEDURE dbo.usp_RoomAmenities_Assign
    @RoomId     INT,
    @AmenityIds NVARCHAR(MAX)
AS
BEGIN
    SET NOCOUNT ON;

    BEGIN TRY
        BEGIN TRANSACTION;

        IF NOT EXISTS (SELECT 1 FROM dbo.Rooms WHERE RoomId = @RoomId)
            THROW 50022, 'Room not found.', 1;

        INSERT INTO dbo.RoomAmenities (RoomId, AmenityId, CreatedAt)
        SELECT DISTINCT @RoomId, TRY_CAST(TRIM(value) AS INT), GETDATE()
        FROM STRING_SPLIT(@AmenityIds, ',')
        WHERE TRY_CAST(TRIM(value) AS INT) IS NOT NULL
          AND TRY_CAST(TRIM(value) AS INT) IN (SELECT AmenityId FROM dbo.Amenities WHERE IsActive = 1)
          AND NOT EXISTS (
              SELECT 1 FROM dbo.RoomAmenities ra
              WHERE ra.RoomId = @RoomId AND ra.AmenityId = TRY_CAST(TRIM(value) AS INT)
          );

        INSERT INTO dbo.AuditLogs (EntityName, EntityId, Action, Details, Timestamp)
        VALUES ('RoomAmenities', @RoomId, 'ASSIGN', 'Bulk assigned amenities to room ' + CAST(@RoomId AS NVARCHAR(10)), GETDATE());

        COMMIT TRANSACTION;

        SELECT 
            a.AmenityId, a.AmenityName, a.Description, a.IconCode, a.IsActive, a.CreatedAt
        FROM dbo.RoomAmenities ra
        JOIN dbo.Amenities a ON ra.AmenityId = a.AmenityId
        WHERE ra.RoomId = @RoomId AND a.IsActive = 1
        ORDER BY a.AmenityName;
    END TRY
    BEGIN CATCH
        IF @@TRANCOUNT > 0
            ROLLBACK TRANSACTION;
        THROW;
    END CATCH;
END;
GO

IF OBJECT_ID('dbo.usp_RoomAmenities_GetByRoomId', 'P') IS NOT NULL
    DROP PROCEDURE dbo.usp_RoomAmenities_GetByRoomId;
GO
CREATE PROCEDURE dbo.usp_RoomAmenities_GetByRoomId
    @RoomId INT
AS
BEGIN
    SET NOCOUNT ON;

    SELECT 
        a.AmenityId, a.AmenityName, a.Description, a.IconCode, a.IsActive, a.CreatedAt
    FROM dbo.RoomAmenities ra
    JOIN dbo.Amenities a ON ra.AmenityId = a.AmenityId
    WHERE ra.RoomId = @RoomId AND a.IsActive = 1
    ORDER BY a.AmenityName;
END;
GO

IF OBJECT_ID('dbo.usp_RoomAmenities_Remove', 'P') IS NOT NULL
    DROP PROCEDURE dbo.usp_RoomAmenities_Remove;
GO
CREATE PROCEDURE dbo.usp_RoomAmenities_Remove
    @RoomId    INT,
    @AmenityId INT
AS
BEGIN
    SET NOCOUNT ON;

    BEGIN TRY
        BEGIN TRANSACTION;

        DELETE FROM dbo.RoomAmenities
        WHERE RoomId = @RoomId AND AmenityId = @AmenityId;

        INSERT INTO dbo.AuditLogs (EntityName, EntityId, Action, Details, Timestamp)
        VALUES ('RoomAmenities', @RoomId, 'REMOVE', 'Removed amenity ' + CAST(@AmenityId AS NVARCHAR(10)) + ' from room ' + CAST(@RoomId AS NVARCHAR(10)), GETDATE());

        COMMIT TRANSACTION;

        SELECT @@ROWCOUNT AS RowsAffected;
    END TRY
    BEGIN CATCH
        IF @@TRANCOUNT > 0
            ROLLBACK TRANSACTION;
        THROW;
    END CATCH;
END;
GO

-- ----------------------------------------------------------------------------
-- MODULE 8: HOTEL SEARCH MODULE PROCEDURE
-- ----------------------------------------------------------------------------

IF OBJECT_ID('dbo.usp_Room_SearchAvailable', 'P') IS NOT NULL
    DROP PROCEDURE dbo.usp_Room_SearchAvailable;
GO
CREATE PROCEDURE dbo.usp_Room_SearchAvailable
    @CheckInDate  DATE,
    @CheckOutDate DATE,
    @Guests       INT            = NULL,
    @RoomTypeId   INT            = NULL,
    @MinPrice     DECIMAL(18, 2) = NULL,
    @MaxPrice     DECIMAL(18, 2) = NULL,
    @AmenityIds   NVARCHAR(MAX)  = NULL,
    @PageNumber   INT            = 1,
    @PageSize     INT            = 10
AS
BEGIN
    SET NOCOUNT ON;

    -- Date Validations
    IF @CheckInDate IS NULL OR @CheckOutDate IS NULL
        THROW 50041, 'CheckInDate and CheckOutDate are required.', 1;

    IF @CheckInDate < CAST(GETDATE() AS DATE)
        THROW 50042, 'CheckInDate cannot be in the past.', 1;

    IF @CheckOutDate <= @CheckInDate
        THROW 50043, 'CheckOutDate must be strictly after CheckInDate.', 1;

    IF @PageNumber < 1 SET @PageNumber = 1;
    IF @PageSize < 1 SET @PageSize = 10;

    -- Amenity filtering (Relational Division)
    DECLARE @ReqAmenities TABLE (AmenityId INT PRIMARY KEY);
    DECLARE @ReqCount INT = 0;

    IF @AmenityIds IS NOT NULL AND LEN(TRIM(@AmenityIds)) > 0
    BEGIN
        INSERT INTO @ReqAmenities (AmenityId)
        SELECT DISTINCT TRY_CAST(TRIM(value) AS INT)
        FROM STRING_SPLIT(@AmenityIds, ',')
        WHERE TRY_CAST(TRIM(value) AS INT) IS NOT NULL;

        SET @ReqCount = (SELECT COUNT(*) FROM @ReqAmenities);
    END

    DECLARE @Nights INT = DATEDIFF(DAY, @CheckInDate, @CheckOutDate);

    SELECT 
        r.RoomId,
        r.RoomNumber,
        r.FloorNumber,
        r.Status,
        rt.RoomTypeId,
        rt.TypeName AS RoomTypeName,
        rt.Description AS RoomTypeDescription,
        rt.BasePricePerNight,
        rt.MaxOccupancy,
        @Nights AS TotalNights,
        (rt.BasePricePerNight * @Nights) AS TotalStayPrice,
        COUNT(*) OVER() AS TotalRecords
    FROM dbo.Rooms r
    JOIN dbo.RoomTypes rt ON r.RoomTypeId = rt.RoomTypeId
    WHERE r.IsActive = 1
      AND rt.IsActive = 1
      AND r.Status <> 'UnderMaintenance'
      -- Overlap detection: Existing.CheckInDate < Requested.CheckOutDate AND Existing.CheckOutDate > Requested.CheckInDate
      AND NOT EXISTS (
          SELECT 1 
          FROM dbo.Bookings b
          WHERE b.RoomId = r.RoomId
            AND b.BookingStatus = 'Confirmed'
            AND b.CheckInDate < @CheckOutDate
            AND b.CheckOutDate > @CheckInDate
      )
      AND (@Guests IS NULL OR rt.MaxOccupancy >= @Guests)
      AND (@RoomTypeId IS NULL OR r.RoomTypeId = @RoomTypeId)
      AND (@MinPrice IS NULL OR rt.BasePricePerNight >= @MinPrice)
      AND (@MaxPrice IS NULL OR rt.BasePricePerNight <= @MaxPrice)
      -- Relational Division: Room must possess ALL requested amenities
      AND (
          @ReqCount = 0 
          OR (
              SELECT COUNT(DISTINCT ra.AmenityId)
              FROM dbo.RoomAmenities ra
              JOIN @ReqAmenities req ON ra.AmenityId = req.AmenityId
              WHERE ra.RoomId = r.RoomId
          ) = @ReqCount
      )
    ORDER BY rt.BasePricePerNight ASC, r.RoomNumber ASC
    OFFSET (@PageNumber - 1) * @PageSize ROWS
    FETCH NEXT @PageSize ROWS ONLY;
END;
GO

-- ----------------------------------------------------------------------------
-- MODULE 9: HOTEL BOOKING MODULE PROCEDURES
-- ----------------------------------------------------------------------------

IF OBJECT_ID('dbo.usp_Booking_Create', 'P') IS NOT NULL
    DROP PROCEDURE dbo.usp_Booking_Create;
GO
CREATE PROCEDURE dbo.usp_Booking_Create
    @UserId           INT,
    @RoomId           INT,
    @CheckInDate      DATE,
    @CheckOutDate     DATE,
    @NumberOfGuests   INT,
    @BookingReference NVARCHAR(50) = NULL OUTPUT,
    @BookingId        INT          = NULL OUTPUT
AS
BEGIN
    SET NOCOUNT ON;

    BEGIN TRY
        BEGIN TRANSACTION;

        -- 1. Date Validations
        IF @CheckInDate < CAST(GETDATE() AS DATE)
            THROW 50051, 'CheckInDate cannot be in the past.', 1;

        IF @CheckOutDate <= @CheckInDate
            THROW 50052, 'CheckOutDate must be after CheckInDate.', 1;

        -- 2. User Validation
        IF NOT EXISTS (SELECT 1 FROM dbo.Users WHERE UserId = @UserId AND IsActive = 1)
            THROW 50053, 'User not found or inactive.', 1;

        -- 3. Room & RoomType check with UPDLOCK, HOLDLOCK
        DECLARE @BasePricePerNight DECIMAL(18, 2);
        DECLARE @MaxOccupancy INT;
        DECLARE @RoomStatus NVARCHAR(30);
        DECLARE @IsRoomActive BIT;
        DECLARE @RoomNumber NVARCHAR(20);
        DECLARE @RoomTypeName NVARCHAR(100);

        SELECT 
            @RoomNumber = r.RoomNumber,
            @RoomStatus = r.Status,
            @IsRoomActive = r.IsActive,
            @BasePricePerNight = rt.BasePricePerNight,
            @MaxOccupancy = rt.MaxOccupancy,
            @RoomTypeName = rt.TypeName
        FROM dbo.Rooms r WITH (UPDLOCK, HOLDLOCK)
        JOIN dbo.RoomTypes rt WITH (UPDLOCK, HOLDLOCK) ON r.RoomTypeId = rt.RoomTypeId
        WHERE r.RoomId = @RoomId;

        IF @RoomStatus IS NULL OR @IsRoomActive = 0
            THROW 50054, 'Room not found or inactive.', 1;

        IF @RoomStatus = 'UnderMaintenance'
            THROW 50055, 'Room is currently under maintenance.', 1;

        IF @NumberOfGuests < 1
            THROW 50056, 'NumberOfGuests must be at least 1.', 1;

        IF @NumberOfGuests > @MaxOccupancy
            THROW 50057, 'NumberOfGuests exceeds maximum occupancy for this room type.', 1;

        -- 4. Overlap Check with UPDLOCK, HOLDLOCK (prevents double-booking race conditions)
        IF EXISTS (
            SELECT 1 
            FROM dbo.Bookings WITH (UPDLOCK, HOLDLOCK)
            WHERE RoomId = @RoomId
              AND BookingStatus = 'Confirmed'
              AND CheckInDate < @CheckOutDate
              AND CheckOutDate > @CheckInDate
        )
        BEGIN
            THROW 50058, 'Room is already booked for the selected dates.', 1;
        END

        -- 5. Calculate Server-Side Total Amount
        DECLARE @Nights INT = DATEDIFF(DAY, @CheckInDate, @CheckOutDate);
        DECLARE @TotalAmount DECIMAL(18, 2) = @Nights * @BasePricePerNight;

        -- 6. Generate Unique Booking Reference: BK-YYYYMMDD-XXXX
        DECLARE @DatePart VARCHAR(8) = CONVERT(VARCHAR(8), GETDATE(), 112);
        DECLARE @UniqueRef NVARCHAR(50);
        DECLARE @IsUnique BIT = 0;

        WHILE @IsUnique = 0
        BEGIN
            SET @UniqueRef = 'BK-' + @DatePart + '-' + UPPER(SUBSTRING(REPLACE(CONVERT(NVARCHAR(36), NEWID()), '-', ''), 1, 6));
            IF NOT EXISTS (SELECT 1 FROM dbo.Bookings WHERE BookingReference = @UniqueRef)
                SET @IsUnique = 1;
        END

        -- 7. Insert Booking
        INSERT INTO dbo.Bookings (
            BookingReference, UserId, RoomId, CheckInDate, CheckOutDate, NumberOfGuests, TotalAmount, BookingStatus, CreatedAt
        )
        VALUES (
            @UniqueRef, @UserId, @RoomId, @CheckInDate, @CheckOutDate, @NumberOfGuests, @TotalAmount, 'Confirmed', GETDATE()
        );

        SET @BookingId = SCOPE_IDENTITY();
        SET @BookingReference = @UniqueRef;

        -- 8. Audit Log
        INSERT INTO dbo.AuditLogs (EntityName, EntityId, Action, UserId, Details, Timestamp)
        VALUES ('Bookings', @BookingId, 'INSERT', @UserId, 'Booking created with reference: ' + @UniqueRef, GETDATE());

        COMMIT TRANSACTION;

        -- Return newly created booking
        SELECT 
            b.BookingId,
            b.BookingReference,
            b.UserId,
            u.FullName AS CustomerName,
            u.Email AS CustomerEmail,
            b.RoomId,
            r.RoomNumber,
            rt.TypeName AS RoomTypeName,
            b.CheckInDate,
            b.CheckOutDate,
            @Nights AS TotalNights,
            b.NumberOfGuests,
            b.TotalAmount,
            b.BookingStatus,
            b.CreatedAt
        FROM dbo.Bookings b
        JOIN dbo.Users u ON b.UserId = u.UserId
        JOIN dbo.Rooms r ON b.RoomId = r.RoomId
        JOIN dbo.RoomTypes rt ON r.RoomTypeId = rt.RoomTypeId
        WHERE b.BookingId = @BookingId;
    END TRY
    BEGIN CATCH
        IF @@TRANCOUNT > 0
            ROLLBACK TRANSACTION;
        THROW;
    END CATCH;
END;
GO

IF OBJECT_ID('dbo.usp_Booking_GetByUser', 'P') IS NOT NULL
    DROP PROCEDURE dbo.usp_Booking_GetByUser;
GO
CREATE PROCEDURE dbo.usp_Booking_GetByUser
    @UserId INT
AS
BEGIN
    SET NOCOUNT ON;

    SELECT 
        b.BookingId,
        b.BookingReference,
        b.UserId,
        u.FullName AS CustomerName,
        u.Email AS CustomerEmail,
        b.RoomId,
        r.RoomNumber,
        rt.TypeName AS RoomTypeName,
        b.CheckInDate,
        b.CheckOutDate,
        DATEDIFF(DAY, b.CheckInDate, b.CheckOutDate) AS TotalNights,
        b.NumberOfGuests,
        b.TotalAmount,
        b.BookingStatus,
        b.CreatedAt
    FROM dbo.Bookings b
    JOIN dbo.Users u ON b.UserId = u.UserId
    JOIN dbo.Rooms r ON b.RoomId = r.RoomId
    JOIN dbo.RoomTypes rt ON r.RoomTypeId = rt.RoomTypeId
    WHERE b.UserId = @UserId
    ORDER BY b.CreatedAt DESC;
END;
GO

IF OBJECT_ID('dbo.usp_Booking_GetByReference', 'P') IS NOT NULL
    DROP PROCEDURE dbo.usp_Booking_GetByReference;
GO
CREATE PROCEDURE dbo.usp_Booking_GetByReference
    @BookingReference NVARCHAR(50)
AS
BEGIN
    SET NOCOUNT ON;

    SELECT 
        b.BookingId,
        b.BookingReference,
        b.UserId,
        u.FullName AS CustomerName,
        u.Email AS CustomerEmail,
        b.RoomId,
        r.RoomNumber,
        rt.TypeName AS RoomTypeName,
        b.CheckInDate,
        b.CheckOutDate,
        DATEDIFF(DAY, b.CheckInDate, b.CheckOutDate) AS TotalNights,
        b.NumberOfGuests,
        b.TotalAmount,
        b.BookingStatus,
        b.CreatedAt,
        c.CancellationId,
        c.CancelledByUserId,
        cu.FullName AS CancelledByName,
        c.CancellationReason,
        c.RefundAmount,
        c.CancellationCharge,
        c.CancelledAt
    FROM dbo.Bookings b
    JOIN dbo.Users u ON b.UserId = u.UserId
    JOIN dbo.Rooms r ON b.RoomId = r.RoomId
    JOIN dbo.RoomTypes rt ON r.RoomTypeId = rt.RoomTypeId
    LEFT JOIN dbo.Cancellations c ON b.BookingId = c.BookingId
    LEFT JOIN dbo.Users cu ON c.CancelledByUserId = cu.UserId
    WHERE b.BookingReference = @BookingReference;
END;
GO

IF OBJECT_ID('dbo.usp_Booking_GetAll', 'P') IS NOT NULL
    DROP PROCEDURE dbo.usp_Booking_GetAll;
GO
CREATE PROCEDURE dbo.usp_Booking_GetAll
    @StartDate  DATE         = NULL,
    @EndDate    DATE         = NULL,
    @Status     NVARCHAR(20) = NULL,
    @PageNumber INT          = 1,
    @PageSize   INT          = 20
AS
BEGIN
    SET NOCOUNT ON;

    IF @PageNumber < 1 SET @PageNumber = 1;
    IF @PageSize < 1 SET @PageSize = 20;

    SELECT 
        b.BookingId,
        b.BookingReference,
        b.UserId,
        u.FullName AS CustomerName,
        u.Email AS CustomerEmail,
        b.RoomId,
        r.RoomNumber,
        rt.TypeName AS RoomTypeName,
        b.CheckInDate,
        b.CheckOutDate,
        DATEDIFF(DAY, b.CheckInDate, b.CheckOutDate) AS TotalNights,
        b.NumberOfGuests,
        b.TotalAmount,
        b.BookingStatus,
        b.CreatedAt,
        COUNT(*) OVER() AS TotalRecords
    FROM dbo.Bookings b
    JOIN dbo.Users u ON b.UserId = u.UserId
    JOIN dbo.Rooms r ON b.RoomId = r.RoomId
    JOIN dbo.RoomTypes rt ON r.RoomTypeId = rt.RoomTypeId
    WHERE (@StartDate IS NULL OR b.CheckInDate >= @StartDate)
      AND (@EndDate IS NULL OR b.CheckOutDate <= @EndDate)
      AND (@Status IS NULL OR b.BookingStatus = @Status)
    ORDER BY b.CreatedAt DESC
    OFFSET (@PageNumber - 1) * @PageSize ROWS
    FETCH NEXT @PageSize ROWS ONLY;
END;
GO

-- ----------------------------------------------------------------------------
-- MODULE 10: HOTEL CANCELLATION MODULE PROCEDURES
-- ----------------------------------------------------------------------------

IF OBJECT_ID('dbo.usp_Booking_Cancel', 'P') IS NOT NULL
    DROP PROCEDURE dbo.usp_Booking_Cancel;
GO
CREATE PROCEDURE dbo.usp_Booking_Cancel
    @BookingId          INT,
    @CancelledByUserId  INT,
    @CancellationReason NVARCHAR(500) = NULL
AS
BEGIN
    SET NOCOUNT ON;

    BEGIN TRY
        BEGIN TRANSACTION;

        DECLARE @CheckInDate DATE;
        DECLARE @TotalAmount DECIMAL(18, 2);
        DECLARE @BookingStatus NVARCHAR(20);
        DECLARE @BookingReference NVARCHAR(50);

        SELECT 
            @CheckInDate = CheckInDate,
            @TotalAmount = TotalAmount,
            @BookingStatus = BookingStatus,
            @BookingReference = BookingReference
        FROM dbo.Bookings WITH (UPDLOCK, HOLDLOCK)
        WHERE BookingId = @BookingId;

        IF @BookingStatus IS NULL
            THROW 50060, 'Booking not found.', 1;

        IF @BookingStatus = 'Cancelled'
            THROW 50061, 'Booking is already cancelled.', 1;

        IF @BookingStatus = 'Completed'
            THROW 50062, 'Cannot cancel a completed booking.', 1;

        IF CAST(GETDATE() AS DATE) >= @CheckInDate
            THROW 50063, 'Cannot cancel booking on or after check-in date.', 1;

        -- Refund Calculation Logic:
        -- > 48 hours before Check-In: 100% Refund (0% cancellation charge)
        -- Between 24 and 48 hours before Check-In: 50% Refund (50% cancellation charge)
        -- Less than 24 hours before Check-In: 0% Refund (100% cancellation charge)
        DECLARE @CheckInDateTime DATETIME2 = CAST(@CheckInDate AS DATETIME2);
        DECLARE @HoursUntilCheckIn DECIMAL(18, 2) = DATEDIFF(MINUTE, GETDATE(), @CheckInDateTime) / 60.0;

        DECLARE @RefundAmount DECIMAL(18, 2);
        DECLARE @CancellationCharge DECIMAL(18, 2);

        IF @HoursUntilCheckIn > 48.0
        BEGIN
            SET @RefundAmount = @TotalAmount;
            SET @CancellationCharge = 0.00;
        END
        ELSE IF @HoursUntilCheckIn >= 24.0
        BEGIN
            SET @RefundAmount = ROUND(@TotalAmount * 0.50, 2);
            SET @CancellationCharge = @TotalAmount - @RefundAmount;
        END
        ELSE
        BEGIN
            SET @RefundAmount = 0.00;
            SET @CancellationCharge = @TotalAmount;
        END

        -- Atomic update BookingStatus = 'Cancelled' AND insert Cancellations record
        UPDATE dbo.Bookings
        SET BookingStatus = 'Cancelled'
        WHERE BookingId = @BookingId;

        INSERT INTO dbo.Cancellations (
            BookingId, CancelledByUserId, CancellationReason, RefundAmount, CancellationCharge, CancelledAt
        )
        VALUES (
            @BookingId, @CancelledByUserId, @CancellationReason, @RefundAmount, @CancellationCharge, GETDATE()
        );

        DECLARE @NewCancellationId INT = SCOPE_IDENTITY();

        INSERT INTO dbo.AuditLogs (EntityName, EntityId, Action, UserId, Details, Timestamp)
        VALUES ('Bookings', @BookingId, 'CANCEL', @CancelledByUserId, 
            'Booking cancelled. Refund: ' + CAST(@RefundAmount AS NVARCHAR(20)) + ', Charge: ' + CAST(@CancellationCharge AS NVARCHAR(20)), 
            GETDATE());

        COMMIT TRANSACTION;

        SELECT 
            c.CancellationId,
            c.BookingId,
            @BookingReference AS BookingReference,
            @TotalAmount AS TotalAmount,
            c.CancelledByUserId,
            u.FullName AS CancelledByName,
            c.CancellationReason,
            c.RefundAmount,
            c.CancellationCharge,
            c.CancelledAt,
            @CheckInDate AS CheckInDate,
            b.CheckOutDate
        FROM dbo.Cancellations c
        JOIN dbo.Bookings b ON c.BookingId = b.BookingId
        JOIN dbo.Users u ON c.CancelledByUserId = u.UserId
        WHERE c.CancellationId = @NewCancellationId;
    END TRY
    BEGIN CATCH
        IF @@TRANCOUNT > 0
            ROLLBACK TRANSACTION;
        THROW;
    END CATCH;
END;
GO

IF OBJECT_ID('dbo.usp_Cancellation_GetDetails', 'P') IS NOT NULL
    DROP PROCEDURE dbo.usp_Cancellation_GetDetails;
GO
CREATE PROCEDURE dbo.usp_Cancellation_GetDetails
    @BookingId INT
AS
BEGIN
    SET NOCOUNT ON;

    SELECT 
        c.CancellationId,
        c.BookingId,
        b.BookingReference,
        b.TotalAmount,
        c.CancelledByUserId,
        u.FullName AS CancelledByName,
        c.CancellationReason,
        c.RefundAmount,
        c.CancellationCharge,
        c.CancelledAt,
        b.CheckInDate,
        b.CheckOutDate
    FROM dbo.Cancellations c
    JOIN dbo.Bookings b ON c.BookingId = b.BookingId
    JOIN dbo.Users u ON c.CancelledByUserId = u.UserId
    WHERE c.BookingId = @BookingId;
END;
GO
