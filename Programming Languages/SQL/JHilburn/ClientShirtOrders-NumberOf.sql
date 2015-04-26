select 'x' as rvalue from Order_Detail as od 
inner join Product p on od.Product_ID = p.Product_ID
inner join Product_Category pc on pc.Product_Category_ID = p.Product_Category_ID
inner join Order_Header oh on od.Order_Header_ID = oh.Order_Header_ID
where oh.Customer_ID = @customerID AND pc.Product_Category_ID = 1 and od.Order_Status_ID not in (select Order_Status_ID from Order_Status where IT_CODE = 'CANCELED')