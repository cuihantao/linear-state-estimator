UPDATE CustomActionAdapter SET ConnectionString='' WHERE AdapterName='MEASUREMENT!SAMPLER';

UPDATE CustomActionAdapter SET ConnectionString='' WHERE AdapterName='LINEAR!STATE!ESTIMATOR';
UPDATE CustomActionAdapter SET Enabled=1 WHERE AdapterName='LINEAR!STATE!ESTIMATOR';

UPDATE CustomActionAdapter SET ConnectionString='' WHERE AdapterName='SNAPSHOT!MANAGER';
UPDATE CustomActionAdapter SET Enabled=1 WHERE AdapterName='SNAPSHOT!MANAGER';