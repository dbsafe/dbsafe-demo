using DbSafe;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SqlDbSafe;
using System;

namespace ProductDAL.Tests
{
    // This test uses a global temp table (GTT) since a GTT is deleted when the connection that created it goes out of scope.
    [TestClass]
    public class ReusingConnectionTest
    {
        private IDbSafeManager _dbSafe;

        public void Initialize(DbSafeManagerConfig dbsafeConfig)
        {
            _dbSafe = SqlDbSafeManager.Initialize(dbsafeConfig, "product-db-test.xml")
                .SetConnectionString("ProductEntities-Test-Framework");

            Console.WriteLine($"IsGlobalConfig: {_dbSafe.Config.IsGlobalConfig}, SerializeTests: {_dbSafe.Config.SerializeTests}, ReuseConnection: {_dbSafe.Config.ReuseConnection}");
        }

        [TestCleanup]
        public void Cleanup()
        {
            _dbSafe?.Completed();
        }

        [TestMethod]
        public void Connection_is_reused()
        {
            var dbsafeConfig = new DbSafeManagerConfig { SerializeTests = true, ReuseConnection = true };
            Initialize(dbsafeConfig);

            _dbSafe.ExecuteScripts("create-global-temp-table");
            _dbSafe.AssertDatasetVsScript("dataset-temp-table-exists", "script-verify-if-temp-table-exists", "TempTableExists");
        }

        [TestMethod]
        public void Connection_is_not_reused()
        {
            var dbsafeConfig = new DbSafeManagerConfig { SerializeTests = true };
            Initialize(dbsafeConfig);

            _dbSafe.ExecuteScripts("create-global-temp-table");
            _dbSafe.AssertDatasetVsScript("dataset-temp-table-does-not-exist", "script-verify-if-temp-table-exists", "TempTableExists");
        }
    }
}
