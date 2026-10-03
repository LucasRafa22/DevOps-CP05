SELECT * FROM Pedidos ORDER BY DataCriacao DESC;
SELECT * FROM ItensPedido ORDER BY PedidoId;
SELECT p.Id, p.ClienteNome, p.ValorTotal,
       SUM(i.Quantidade * i.PrecoUnitario) AS TotalCalculado
FROM Pedidos p
LEFT JOIN ItensPedido i ON i.PedidoId = p.Id
GROUP BY p.Id, p.ClienteNome, p.ValorTotal;
