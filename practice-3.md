
а) Вывести суммарный объём продаж (amount) за каждый месяц 2023 года
SELECT
    to_char(date_trunc('month', order_date), 'YYYY-MM') AS month,
    SUM(amount) AS total_amount
FROM orders
WHERE EXTRACT(YEAR FROM order_date) = 2023
GROUP BY date_trunc('month', order_date)
ORDER BY month;

б) Найти всех клиентов, у которых средний чек превышает средний чек по всем клиентам.

SELECT customer_id
FROM orders
GROUP BY customer_id
HAVING AVG(amount) > (SELECT AVG(amount) FROM orders);


в папке img представлены скриншоты создания таблицы,заполенения данных и выполнениния работы 2 скриптов