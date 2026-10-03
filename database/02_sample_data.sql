DECLARE @PedidoId UNIQUEIDENTIFIER = NEWID();

INSERT INTO Pedidos (Id, ClienteNome, DataCriacao, ValorTotal)
VALUES (@PedidoId, 'Cliente Demonstração', SYSUTCDATETIME(), 35.00);

INSERT INTO ItensPedido (Id, PedidoId, Descricao, Quantidade, PrecoUnitario)
VALUES
(NEWID(), @PedidoId, 'Plano mensal Dimdim', 1, 25.00),
(NEWID(), @PedidoId, 'Serviço adicional', 1, 10.00);

UPDATE Pedidos
SET ValorTotal = (
    SELECT SUM(Quantidade * PrecoUnitario)
    FROM ItensPedido
    WHERE PedidoId = @PedidoId
)
WHERE Id = @PedidoId;
