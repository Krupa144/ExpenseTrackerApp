APP_PROJECT=ExpenseTrackerApp
TEST_PROJECT=ExpenseTracker.Tests
ENV_FILE=.env
DOTNET=dotnet

# Uruchom aplikację (restore + build + run)
run:
	$(DOTNET) restore
	$(DOTNET) build $(APP_PROJECT)
	$(DOTNET) run --project $(APP_PROJECT)
#Testy
test:
	$(DOTNET) test $(TEST_PROJECT)
