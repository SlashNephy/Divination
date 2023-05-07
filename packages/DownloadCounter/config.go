package main

import "github.com/caarlos0/env/v8"

type Config struct {
	DatabaseDSN string `env:"DATABASE_DSN"`
	Address     string `env:"ADDRESS"`
}

func LoadConfig() (*Config, error) {
	var config Config
	if err := env.Parse(&config); err != nil {
		return nil, err
	}

	return &config, nil
}
