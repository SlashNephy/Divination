package main

import (
	"fmt"
	"net/http"

	"github.com/labstack/echo/v4"
)

type Controller struct {
	repo   *Repository
	config *Config
}

func NewController(repo *Repository, config *Config) *Controller {
	return &Controller{
		repo,
		config,
	}
}

func (c *Controller) RegisterRoutes(e *echo.Echo) {
	e.GET("/", c.GetIndex)
	e.GET("/statistics", c.GetStatistics)
	e.GET("/:channel/:plugin_id", c.GetPlugin)
}

func (c *Controller) GetIndex(ctx echo.Context) error {
	return ctx.Redirect(http.StatusFound, "https://xiv.starry.blue")
}

func (c *Controller) GetStatistics(ctx echo.Context) error {
	counts, err := c.repo.ListDownloadCounts(ctx.Request().Context())
	if err != nil {
		ctx.Logger().Error(err)
		return echo.ErrInternalServerError
	}

	return ctx.JSON(http.StatusOK, counts)
}

func (c *Controller) GetPlugin(ctx echo.Context) error {
	var params struct {
		Channel  string `param:"channel"`
		PluginID string `param:"plugin_id"`
	}
	if err := ctx.Bind(&params); err != nil {
		return err
	}

	if params.Channel != "stable" && params.Channel != "testing" {
		return echo.ErrNotFound
	}

	url := fmt.Sprintf("https://xiv.starry.blue/plugins/%s/%s/latest.zip", params.Channel, params.PluginID)
	if !checkRemoteResource(url) {
		ctx.Logger().Warnf("remote resource not found: %s", url)
		return echo.ErrNotFound
	}

	if err := c.repo.IncrementDownloadCount(ctx.Request().Context(), params.PluginID); err != nil {
		ctx.Logger().Error(err)
		return echo.ErrInternalServerError
	}

	return ctx.Redirect(http.StatusFound, url)
}

func checkRemoteResource(url string) bool {
	request, err := http.NewRequest(http.MethodHead, url, nil)
	if err != nil {
		return false
	}

	request.Header.Set("User-Agent", "Divination/DownloadCounter (+https://github.com/SlashNephy/Divination)")

	response, err := http.DefaultClient.Do(request)
	if err != nil {
		return false
	}

	return 200 <= response.StatusCode && response.StatusCode < 300
}
