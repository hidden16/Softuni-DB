CREATE TABLE Clients(
ClientId INT PRIMARY KEY IDENTITY NOT NULL
,FirstName NVARCHAR(50) NOT NULL
,LastName NVARCHAR(50) NOT NULL
,Phone VARCHAR(12)
,CHECK (LEN(Phone) = 12)
)

CREATE TABLE Mechanics(
MechanicId INT PRIMARY KEY IDENTITY NOT NULL
,FirstName NVARCHAR(50) NOT NULL
,LastName NVARCHAR(50) NOT NULL
,[Address] NVARCHAR(255) NOT NULL
)

CREATE TABLE Models(
ModelId INT PRIMARY KEY IDENTITY NOT NULL
,[Name] NVARCHAR(50) UNIQUE NOT NULL
)

CREATE TABLE Jobs(
JobId INT PRIMARY KEY IDENTITY NOT NULL
,ModelId INT FOREIGN KEY REFERENCES Models(ModelId) NOT NULL
,[Status] NVARCHAR(11) DEFAULT('Pending') NOT NULL
,CHECK([Status] = 'Pending' 
	OR [Status] = 'In Progress' 
	OR [Status] = 'Finished')
,ClientId INT FOREIGN KEY REFERENCES Clients(ClientId) NOT NULL
,MechanicId INT FOREIGN KEY REFERENCES Mechanics(MechanicId)
,IssueDate DATE NOT NULL
,FinishDate DATE
)

CREATE TABLE Orders(
OrderId INT PRIMARY KEY IDENTITY NOT NULL
,JobId INT FOREIGN KEY REFERENCES Jobs(JobId)
,IssueDate DATE
,Delivered BIT DEFAULT(0) NOT NULL
)

CREATE TABLE Vendors(
VendorId INT PRIMARY KEY IDENTITY NOT NULL
,[Name] NVARCHAR(50) UNIQUE NOT NULL
)

CREATE TABLE Parts(
PartId INT PRIMARY KEY IDENTITY NOT NULL
,SerialNumber NVARCHAR(50) UNIQUE NOT NULL
,[Description] NVARCHAR(255)
,Price DECIMAL(6,2) NOT NULL
,CHECK (Price > 0 )
,VendorId INT FOREIGN KEY REFERENCES Vendors(VendorId) NOT NULL
,StockQty INT DEFAULT(0) NOT NULL
,CHECK (StockQty > 0)
)

CREATE TABLE OrderParts(
OrderId INT FOREIGN KEY REFERENCES Orders(OrderId) NOT NULL
,PartId INT FOREIGN KEY REFERENCES Parts(PartId) NOT NULL
,Quantity INT DEFAULT(1) NOT NULL
,CHECK(Quantity > 0)
,PRIMARY KEY (OrderId,PartId)
)

CREATE TABLE PartsNeeded(
JobId INT FOREIGN KEY REFERENCES Jobs(JobId) NOT NULL
,PartId INT FOREIGN KEY REFERENCES Parts(PartId) NOT NULL
,Quantity INT DEFAULT(1) NOT NULL
,CHECK(Quantity > 0)
,PRIMARY KEY (JobId,PartId)
)