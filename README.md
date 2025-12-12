# SagaChoregraphyPattern
Flow (choreography):
•	Client calls OrderService API -> OrderService saves an Order (Pending) and publishes OrderCreated.
•	ProductService consumes OrderCreated -> attempts to reserve stock:
•	If success -> publishes ProductReserved.
•	If failure -> publishes OrderFailed (reason: insufficient stock).
•	OrderService consumes ProductReserved:
•	If order has Fail flag -> simulate failure: mark order Failed and publish OrderFailed (this triggers compensation).
•	Else mark order Created and publish OrderCompleted.
•	ProductService consumes OrderFailed -> reverts (compensates) product reservation.
•	EmailService consumes OrderCompleted and OrderFailed and writes status to console.


ProductService
•	Purpose: consume OrderCreated, reserve product stock (DB), publish ProductReserved or OrderFailed. Also consume OrderFailed for compensation (revert stock).

OrderService
•	Purpose: API to create orders (persist Pending + publish OrderCreated) and consumers to react to ProductReserved and OrderFailed.

EmailService
•	Purpose: consumes final events and writes to console (demo email).

Notes and quick setup tips
•	Add the Contracts project as a class library referenced by the three services.
•	Required NuGet:
•	MassTransit
•	MassTransit.RabbitMQ
•	Microsoft.EntityFrameworkCore
•	Microsoft.EntityFrameworkCore.SqlServer
•	Microsoft.EntityFrameworkCore.Tools
•	RabbitMQ: ensure a RabbitMQ broker is running and credentials/host match appsettings.json.
•	SSMS / SQL Server: replace the connection strings with your real server/database/credentials.
•	To simulate the "order fails after product reserved" scenario, call OrderService POST with "fail": true. Example JSON: { "productId": "11111111-1111-1111-1111-111111111111", "quantity": 1, "fail": true }

##############################################Docker##############################################
docker run -d --hostname rabbitmq-host --name rabbitmq -p 5672:5672 -p 15672:15672 -e RABBITMQ_DEFAULT_USER=guest -e RABBITMQ_DEFAULT_PASS=guest rabbitmq:3-management
docker exec -it rabbitmq rabbitmqctl list_queues -p /  (gives list of queues) or
curl -u guest:guest http://localhost:15672/api/queues   (gives list of queues)
curl -u guest:guest http://localhost:15672/api/exchanges  (gives list of exchanges)


