-- +goose Up
-- +goose StatementBegin
CREATE TABLE statistics (
  plugin_id TEXT NOT NULL,
  downloads INT NOT NULL,
  PRIMARY KEY (plugin_id)
);
ALTER TABLE statistics ADD CHECK ( downloads >= 0 );
-- +goose StatementEnd

-- +goose Down
-- +goose StatementBegin
DROP TABLE statistics;
-- +goose StatementEnd
