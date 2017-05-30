GO
PRINT 'Creating Categories...'

GO
USE [Auction]

INSERT INTO [Category] VALUES (1, '����', null); 
INSERT INTO [Category] VALUES (2, '��������', null); 
INSERT INTO [Category] VALUES (3, '������', null); 
INSERT INTO [Category] VALUES (4, '�����', null); 
INSERT INTO [Category] VALUES (5, '���������', null); 
INSERT INTO [Category] VALUES (6, '�������', null); 
INSERT INTO [Category] VALUES (7, '������', null); 
INSERT INTO [Category] VALUES (8, '��������', null); 
INSERT INTO [Category] VALUES (9, '����', null); 

INSERT INTO [CategoryFeature] VALUES ('680da92a-a084-4cc7-8ad5-3a55f613baad', '�����', 1, 'XS,S,M,L,XL,XXL'); 
INSERT INTO [CategoryFeature] VALUES ('efc87abf-69e8-4da8-9274-c99d3b953824', '���', 1, '�����,��������,�����,�������,�������/�����,ϳ����'); 
INSERT INTO [CategoryFeature] VALUES ('3a1efa3a-8372-4d6e-89ab-466db7695ee9', '����', 1, '�����-����,����������'); 
INSERT INTO [CategoryFeature] VALUES ('2a44f607-f4de-40f1-9239-f7763fa9e4c1', '���', 2, '��������,�������,�������,��� �������,����'); 
INSERT INTO [CategoryFeature] VALUES ('831eb3c7-a24f-45ee-8f5e-42b1993a49ee', '���', 5, '�����������,������'); 
INSERT INTO [CategoryFeature] VALUES ('ac5fc19f-925d-40a3-ac4f-f2ba9ef83a59', '������', 8, 'Apple,Nokia,Samsung,Google,LG,Xiomi,Other'); 
INSERT INTO [CategoryFeature] VALUES ('b0687548-f958-4d9c-9cac-b8281fcc6f60', 'г�', 8, '�� 2010,2010,2011,2012,2013,2014,2015,2016,2017'); 
INSERT INTO [CategoryFeature] VALUES ('66fa1994-5118-4de6-ae9b-dce9e7f95f4d', '��''����', 8, 'SD-cart,����� 8GB,8GB,16GB,32GB,64GB,128GB,256GB,������ 256GB'); 

PRINT 'Categories created'
