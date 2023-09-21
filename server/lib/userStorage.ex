defmodule ElixirServer.UserStorage do
  @moduledoc false
  use Agent

  defstruct [users: %{}]

  def start_link(state) do
    Agent.start_link(fn -> %__MODULE__{state | users: %{}} end, name: __MODULE__)
  end

  def store_player(player_name, player) do
    Agent.update(__MODULE__, fn state ->
      %__MODULE__{state | users: Map.put(state.users, player_name, player)}
    end)
  end

  def get_player(player_name) do
    Agent.get(__MODULE__, fn state ->
      Map.get(state.users, player_name)
    end)
  end

  def delete_player(player_name) do
    Agent.update(__MODULE__, fn state ->
      %__MODULE__{state | users: Map.drop(state.users, [player_name])}
    end)
  end

  def get_all_players() do
    Agent.get(__MODULE__, fn state ->
      Enum.map(state.users, fn {player_name} ->
        ElixirServer.User.get_player(player_name)
      end)
    end)
  end

end
