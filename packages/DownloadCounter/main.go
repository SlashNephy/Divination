package main

import (
	"context"
	"net/http"
	"os"
	"os/signal"
	"time"

	"github.com/labstack/echo/v4"
	"github.com/labstack/gommon/log"
)

func main() {
	e := echo.New()
	e.Logger.SetLevel(log.INFO)

	config, err := LoadConfig()
	if err != nil {
		e.Logger.Fatal(err)
	}

	db, err := ConnectToDatabase(config.DatabaseDSN)
	if err != nil {
		e.Logger.Fatal(err)
	}

	if err = MigrateDatabase(db); err != nil {
		e.Logger.Fatal(err)
	}

	repo := NewRepository(db)
	controller := NewController(repo, config)
	controller.RegisterRoutes(e)

	go func() {
		if err = e.Start(config.Address); err != nil && err != http.ErrServerClosed {
			e.Logger.Fatal("shutting down the server")
		}
	}()

	quit := make(chan os.Signal, 1)
	signal.Notify(quit, os.Interrupt)
	<-quit

	ctx, cancel := context.WithTimeout(context.Background(), 10*time.Second)
	defer cancel()

	if err := e.Shutdown(ctx); err != nil {
		e.Logger.Fatal(err)
	}
}
