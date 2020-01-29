SELECT
    ROWNUM AS id,
    CASE round(dbms_random.value(1, 3))
        WHEN 1   THEN
            'Miss'
        WHEN 2   THEN
            'Mr'
        WHEN 3   THEN
            'Mrs'
    END AS "Title",
    'firstname-' || ROWNUM AS "FirstName",
    'surname-' || ROWNUM AS "Surname",
    CASE round(dbms_random.value(1, 3))
        WHEN 1   THEN
            'Standard Cover'
        WHEN 2   THEN
            'Enhanced Cover'
        WHEN 3   THEN
            'Special Cover'
    END AS "ProductName",
    round(dbms_random.value(10000, 99999), 2) "PayoutAmount",
    round(dbms_random.value(100, 199), 2) "AnnualPremium"
FROM
    dual
CONNECT BY
    level <= 100;