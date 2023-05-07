package main

import (
	"context"

	"github.com/doug-martin/goqu/v9"
	"github.com/jmoiron/sqlx"
)

var dialect = goqu.Dialect("postgres")

type Repository struct {
	db *sqlx.DB
}

func NewRepository(db *sqlx.DB) *Repository {
	return &Repository{
		db,
	}
}

func (r *Repository) IncrementDownloadCount(ctx context.Context, pluginID string) error {
	query, _, err := dialect.Insert("statistics").
		Rows(goqu.Record{
			"plugin_id": pluginID,
			"downloads": 1,
		}).
		OnConflict(goqu.DoUpdate("plugin_id", goqu.Record{
			"downloads": goqu.L("?.? + ?", goqu.T("statistics"), goqu.C("downloads"), 1),
		})).
		ToSQL()
	if err != nil {
		return err
	}

	_, err = r.db.ExecContext(ctx, query)
	return err
}

func (r *Repository) ListDownloadCounts(ctx context.Context) (map[string]uint, error) {
	var rows []struct {
		PluginID  string `db:"plugin_id"`
		Downloads uint   `db:"downloads"`
	}

	query, _, err := dialect.Select(&rows).From("statistics").ToSQL()
	if err != nil {
		return nil, err
	}

	if err = r.db.SelectContext(ctx, &rows, query); err != nil {
		return nil, err
	}

	results := make(map[string]uint)
	for _, row := range rows {
		results[row.PluginID] = row.Downloads
	}

	return results, nil
}
