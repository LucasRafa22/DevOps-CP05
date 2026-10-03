-- Dimdim WebApp - Azure SQL Database
-- Master-detail: Pedidos -> ItensPedido

CREATE TABLE Pedidos (
    Id UNIQUEIDENTIFIER NOT NULL
        CONSTRAINT PK_Pedidos PRIMARY KEY,
    ClienteNome NVARCHAR(120) NOT NULL,
    DataCriacao DATETIME2 NOT NULL,
    ValorTotal DECIMAL(18,2) NOT NULL
        CONSTRAINT DF_Pedidos_ValorTotal DEFAULT 0
);

CREATE TABLE ItensPedido (
    Id UNIQUEIDENTIFIER NOT NULL
        CONSTRAINT PK_ItensPedido PRIMARY KEY,
    PedidoId UNIQUEIDENTIFIER NOT NULL,
    Descricao NVARCHAR(160) NOT NULL,
    Quantidade INT NOT NULL
        CONSTRAINT CK_ItensPedido_Quantidade CHECK (Quantidade > 0),
    PrecoUnitario DECIMAL(18,2) NOT NULL
        CONSTRAINT CK_ItensPedido_Preco CHECK (PrecoUnitario >= 0),
    CONSTRAINT FK_ItensPedido_Pedidos
        FOREIGN KEY (PedidoId) REFERENCES Pedidos(Id)
        ON DELETE CASCADE
);

CREATE INDEX IX_ItensPedido_PedidoId ON ItensPedido(PedidoId);
