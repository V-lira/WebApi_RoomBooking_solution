# Web API бронирование переговорных комнат
------------------------------------------------------
О проекте

Внутренний REST API для системы бронирования переговорных. Сотрудники бронируют комнаты, оператор управляет статусами, а клиент получает данные в удобном виде - с названием комнаты и ФИО сотрудника, а не «голыми» id

Хранилище - в памяти приложения, без БД. При старте создаются 6 комнат (одна неактивная), 8 сотрудников и 10 броней с разными статусами — чтобы сразу можно было проверять работу API.

----------------------------------------------------------------------------------------------
Стек
-------------
ASP.NET Web API 2, .NET Framework 4.8

Эндпоинты
-------

<img width="1478" height="205" alt="image" src="https://github.com/user-attachments/assets/e51ee637-3c21-4ce9-90ce-d9f853cacd9d" />

GET /api/rooms - все комнаты

GET /api/rooms/{id} - одна

POST /api/rooms - создать

PUT /api/rooms/{id} - изменить

PUT /api/rooms/{id}/deactivate - сделать неактивной

GET /api/rooms/available?start=&end=&participants=&projector=&whiteboard= - свободные на интервал

<img width="1480" height="283" alt="image" src="https://github.com/user-attachments/assets/08030538-cc18-413a-8c64-ac62b46b35e8" />

GET /api/employees, GET /api/employees/{id}

POST /api/employees, PUT /api/employees/{id}


GET /api/bookings?employeeId=&roomId=&date=&status= -список с фильтрами

GET /api/bookings/{id}

POST /api/bookings

PUT /api/bookings/{id}

DELETE /api/bookings/{id}

PUT /api/bookings/{id}/status

GET /api/bookings/{id}/history


Функционал
---
Название комнаты и ФИО/Email сотрудника не пустые, Capacity > 0, дубликаты - 400

Бронь: Start < End, Start не в прошлом, ParticipantsCount > 0, Topic не пустое - иначе 400

Комната неактивна, участников больше вместимости, интервал пересекается - 409

Пересечение считается как newStart < existingEnd && newEnd > existingStart. Соседние интервалы (конец = начало следующего) - не пересечение

Статусы: Planned -> Started | Cancelled, Started -> Finished. Дальше -409

Удалять можно только Planned и Cancelled. Started/Finished -> 409

Запрос к несуществующему id - 404
