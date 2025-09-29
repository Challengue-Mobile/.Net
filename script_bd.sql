-- script_bd.sql | Sprint 3 (DevOps & Cloud)
-- DDL mínimo para demonstrar CRUD (Oracle)

-- DROP TABLE BEACONS CASCADE CONSTRAINTS;

CREATE TABLE BEACONS (
  ID          NUMBER         PRIMARY KEY,
  CODIGO      VARCHAR2(50)   NOT NULL,
  DESCRICAO   VARCHAR2(200),
  CREATED_AT  DATE           DEFAULT SYSDATE
);

COMMENT ON TABLE BEACONS IS 'Tabela de beacons (rastreador) para demo CRUD';
COMMENT ON COLUMN BEACONS.ID IS 'Identificador único';
COMMENT ON COLUMN BEACONS.CODIGO IS 'Código do beacon';
COMMENT ON COLUMN BEACONS.DESCRICAO IS 'Descrição';
COMMENT ON COLUMN BEACONS.CREATED_AT IS 'Data de criação';

-- Inserções mínimas (exigência de 2 registros):
INSERT INTO BEACONS (ID, CODIGO, DESCRICAO) VALUES (1, 'B001', 'Beacon moto X');
INSERT INTO BEACONS (ID, CODIGO, DESCRICAO) VALUES (2, 'B002', 'Beacon moto Y');
COMMIT;
