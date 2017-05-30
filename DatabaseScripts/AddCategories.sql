GO
PRINT 'Creating Categories...'

GO
USE [Auction]

INSERT INTO [Category] VALUES (1, 'Одяг', null); 
INSERT INTO [Category] VALUES (2, 'Прикраси', null); 
INSERT INTO [Category] VALUES (3, 'Квитки', null); 
INSERT INTO [Category] VALUES (4, 'Спорт', null); 
INSERT INTO [Category] VALUES (5, 'Косметика', null); 
INSERT INTO [Category] VALUES (6, 'Гаджети', null); 
INSERT INTO [Category] VALUES (7, 'Взуття', null); 
INSERT INTO [Category] VALUES (8, 'Телефони', null); 
INSERT INTO [Category] VALUES (9, 'Меблі', null); 

INSERT INTO [CategoryFeature] VALUES ('680da92a-a084-4cc7-8ad5-3a55f613baad', 'Розмір', 1, 'XS,S,M,L,XL,XXL'); 
INSERT INTO [CategoryFeature] VALUES ('efc87abf-69e8-4da8-9274-c99d3b953824', 'Тип', 1, 'Сукня,Футболка,Штани,Спідниця,Сорочка/Блуза,Піджак'); 
INSERT INTO [CategoryFeature] VALUES ('3a1efa3a-8372-4d6e-89ab-466db7695ee9', 'Колір', 1, 'Сорно-білий,КОльоровий'); 
INSERT INTO [CategoryFeature] VALUES ('2a44f607-f4de-40f1-9239-f7763fa9e4c1', 'Тип', 2, 'Каблучка,Сережки,Намисто,Для волосся,Інше'); 
INSERT INTO [CategoryFeature] VALUES ('831eb3c7-a24f-45ee-8f5e-42b1993a49ee', 'Тип', 5, 'Декоративна,Догляд'); 
INSERT INTO [CategoryFeature] VALUES ('ac5fc19f-925d-40a3-ac4f-f2ba9ef83a59', 'Модель', 8, 'Apple,Nokia,Samsung,Google,LG,Xiomi,Other'); 
INSERT INTO [CategoryFeature] VALUES ('b0687548-f958-4d9c-9cac-b8281fcc6f60', 'Рік', 8, 'До 2010,2010,2011,2012,2013,2014,2015,2016,2017'); 
INSERT INTO [CategoryFeature] VALUES ('66fa1994-5118-4de6-ae9b-dce9e7f95f4d', 'Па''мять', 8, 'SD-cart,Менше 8GB,8GB,16GB,32GB,64GB,128GB,256GB,Більше 256GB'); 

PRINT 'Categories created'
