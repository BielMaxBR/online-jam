# código inspirado em https://github.com/sergioaugrod/uai_shot/blob/master/lib/uai_shot/store/bullet.ex
defmodule Server.Store.Arrow do
  use Agent

  def start_link(state \\ %{}) do
    Agent.start_link(fn -> state end, name: __MODULE__)
  end

  def all do
    Agent.get(__MODULE__, fn arrows ->
      arrows
      |> Map.to_list()
      |> Enum.map(&elem(&1, 1))
    end)
  end

  def put(arrow) do
    Agent.update(__MODULE__, &Map.put(&1, arrow.id, arrow))
  end

  def get(arrow_id) do
    Agent.get(__MODULE__, &Map.get(&1, arrow_id, default_attrs(arrow_id)))
  end

  def delete(arrow_id) do
    Agent.update(__MODULE__, &Map.delete(&1, arrow_id))
  end

  defp default_attrs(arrow_id) do
    %{id: arrow_id}
  end
end
