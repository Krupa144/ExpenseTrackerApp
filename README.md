# ExpenseTrackerApp

Prosta aplikacja desktopowa **WPF (.NET 8)** do zarządzania wydatkami osobistymi.

---

## Opis projektu

Aplikacja umożliwia:

* dodawanie i usuwanie wydatków,
* zarządzanie kategoriami,
* walidację danych w formularzach,
* zapis danych w lokalnej bazie **SQLite**,
* wyświetlanie komunikatów błędów użytkownikowi.

Interfejs składa się z kilku widoków (okno główne, formularze, lista kategorii).

---

## Zrealizowane wymagania

* minimum dwa widoki aplikacji,
* formularz z walidacją danych,
* lista elementów z dodawaniem i usuwaniem,
* zapis danych lokalnie,
* obsługa błędów i komunikaty dla użytkownika,
* uporządkowany interfejs użytkownika.

---

## Funkcje dodatkowe

* komunikacja z zewnętrznymi API:

* kursy walut (API NBP),
* cena Bitcoina (API CoinGecko),
* asynchroniczne operacje,
* testy jednostkowe.

---

## Uruchomienie

Wymagania:

* .NET 8 SDK
* Windows

```bash
dotnet restore
dotnet build
dotnet run --project ExpenseTrackerApp
```

Lub przy użyciu Makefile (Git Bash):

```bash
make run
make dev
make test
```
