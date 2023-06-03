CREATE DATABASE Taller
GO

USE Taller;
GO

Create Table Clientes (
	IdCliente INT primary key identity,
	CedulaCliente INT Unique not null,
	NombreCliente varchar(50) not null,
	ApellidoCliente varchar(50) not null,
	DireccionCliente varchar(80) not null,
	TelefonoCliente varchar(15) not null
);
GO

Create Table Productos (
	IdProducto Int Identity primary key,
	CodigoProducto int unique not null,
	NombreProducto varchar(50) not null,
	PrecioProducto float not null,
	CantidadProducto int not null
);
GO

Create Table Ventas (
    IdVenta int primary key identity,
    FechaVenta datetime NOT NULL default getdate(),
    ClienteId int NOT NULL,
    TotalVenta decimal(10, 2) NOT NULL,
    foreign key (ClienteId) references Clientes (IdCliente)
);
GO

Create Table Facturas (
    IdFactura int primary key identity,
    VentaId int NOT NULL,
    IdProductoF int NOT NULL,
    Cantidad int NOT NULL,
    Valor decimal(10, 2) NOT NULL,
    foreign key (VentaId) references Ventas (IdVenta),
    foreign key (IdProductoF) references Productos (IdProducto)
);
GO

