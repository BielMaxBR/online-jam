# código baseado em https://github.com/sergioaugrod/uai_shot/blob/master/lib/uai_shot/store/player.ex
defmodule Server.Store.Player do
  use Agent

  def start_link(state \\ %{}) do
    Agent.start_link(fn -> state end, name: __MODULE__)
  end

  def all do
    Agent.get(__MODULE__, fn players ->
      players
      |> Map.to_list()
      |> Enum.map(&elem(&1, 1))
    end)
  end

  def put(player) do
    Agent.update(__MODULE__, &Map.put(&1, player.id, player))
  end

  def get(player_id) do
    Agent.get(__MODULE__, &Map.get(&1, player_id, default_attrs(player_id)))
  end

  def delete(player_id) do
    Agent.update(__MODULE__, &Map.delete(&1, player_id))
  end

  defp default_attrs(player_id) do
    %{id: player_id, nickname: player_id}
  end
end
