-- ===========================================
-- SCRIPT DDL - MOTTOTH TRACKING
-- Sistema de Rastreamento de Frotas
-- ===========================================

-- Tabela de Beacons (Dispositivos de rastreamento)
CREATE TABLE Beacons (
    Id INT PRIMARY KEY IDENTITY(1,1),
    Identificador NVARCHAR(100) NOT NULL UNIQUE,
    Localizacao NVARCHAR(255),
    Ativo BIT NOT NULL DEFAULT 1,
    DataCriacao DATETIME2 DEFAULT GETDATE()
);

EXEC sp_addextendedproperty 
    @name = N'MS_Description', @value = 'Dispositivos beacons para rastreamento',
    @level0type = N'SCHEMA', @level0name = 'dbo',
    @level1type = N'TABLE', @level1name = 'Beacons';

-- Tabela de Usuários
CREATE TABLE Usuarios (
    Id INT PRIMARY KEY IDENTITY(1,1),
    Nome NVARCHAR(200) NOT NULL,
    Email NVARCHAR(255) NOT NULL UNIQUE,
    Senha NVARCHAR(500) NOT NULL,
    Cargo NVARCHAR(100),
    DataCriacao DATETIME2 DEFAULT GETDATE()
);

EXEC sp_addextendedproperty 
    @name = N'MS_Description', @value = 'Usuários do sistema',
    @level0type = N'SCHEMA', @level0name = 'dbo',
    @level1type = N'TABLE', @level1name = 'Usuarios';

-- Tabela de Veículos
CREATE TABLE Veiculos (
    Id INT PRIMARY KEY IDENTITY(1,1),
    Placa NVARCHAR(20) NOT NULL UNIQUE,
    Modelo NVARCHAR(100),
    Marca NVARCHAR(100),
    Ano INT,
    UsuarioId INT,
    DataCriacao DATETIME2 DEFAULT GETDATE(),
    FOREIGN KEY (UsuarioId) REFERENCES Usuarios(Id)
);

EXEC sp_addextendedproperty 
    @name = N'MS_Description', @value = 'Veículos da frota',
    @level0type = N'SCHEMA', @level0name = 'dbo',
    @level1type = N'TABLE', @level1name = 'Veiculos';

-- Tabela de Movimentações
CREATE TABLE Movimentacoes (
    Id INT PRIMARY KEY IDENTITY(1,1),
    VeiculoId INT NOT NULL,
    BeaconId INT NOT NULL,
    Tipo NVARCHAR(50) NOT NULL,
    Latitude DECIMAL(10,7),
    Longitude DECIMAL(10,7),
    DataMovimentacao DATETIME2 NOT NULL DEFAULT GETDATE(),
    FOREIGN KEY (VeiculoId) REFERENCES Veiculos(Id),
    FOREIGN KEY (BeaconId) REFERENCES Beacons(Id)
);

EXEC sp_addextendedproperty 
    @name = N'MS_Description', @value = 'Histórico de movimentações dos veículos',
    @level0type = N'SCHEMA', @level0name = 'dbo',
    @level1type = N'TABLE', @level1name = 'Movimentacoes';

-- Índices para melhor performance
CREATE INDEX IX_Movimentacoes_VeiculoId ON Movimentacoes(VeiculoId);
CREATE INDEX IX_Movimentacoes_BeaconId ON Movimentacoes(BeaconId);
CREATE INDEX IX_Movimentacoes_DataMovimentacao ON Movimentacoes(DataMovimentacao);
CREATE INDEX IX_Veiculos_UsuarioId ON Veiculos(UsuarioId);
