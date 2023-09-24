# código copiado de https://github.com/sergioaugrod/uai_shot/blob/master/lib/uai_shot_web/channels/game_channel.ex
defmodule ServerWeb.GameChannel do
  use ServerWeb, :channel

  alias Server.Store.{Arrow, Player}

  def join("game:lobby", _message, socket) do
    send(self(), :after_join)
    {:ok, %{player_id: socket.assigns.player_id}, socket}
  end

  def join("game:" <> _private_game_id, _params, _socket) do
    {:error, %{reason: "unauthorized"}}
  end

  def handle_info(:after_join, socket) do
    push(socket, "ping", %{})
    {:noreply, socket}
  end

  def handle_in("new_player", state, socket) do
    state = format_state(state)
    nickname = socket.assigns.nickname
    player_id = socket.assigns.player_id

    state
    |> Map.put(:id, player_id)
    |> Map.put(:nickname, nickname)
    |> Player.put()

    # Ranking.put(%{player_id: socket.assigns.player_id, nickname: nickname, value: 0})

    # broadcast(socket, "update_arrows", %{arrows: Arrow.all()})
    broadcast(socket, "update_players", %{players: Player.all()})
    # broadcast(socket, "update_ranking", %{ranking: Ranking.all()})

    {:noreply, socket}
  end

  def handle_in("move_player", state, socket) do
    state
    |> format_state
    |> Map.put(:id, socket.assigns.player_id)
    |> Map.put(:nickname, socket.assigns.nickname)
    |> Player.put()

    broadcast(socket, "update_players", %{players: Player.all()})

    {:noreply, socket}
  end

  def handle_in("shoot_arrow", state, socket) do
    arrow = state
    |> format_state
    |> Map.put(:id, socket.assigns.player_id)
    # |> Arrow.put()

    # IO.inspect(arrow)
    broadcast(socket, "update_arrows", arrow)
    {:noreply, socket}
  end

  def terminate(_msg, socket) do
    player_id = socket.assigns.player_id
    Player.delete(player_id)
    # Ranking.delete(player_id)
    IO.puts("#{IO.ANSI.red()}left player_id: #{player_id}#{IO.ANSI.white()}")
    broadcast(socket, "update_players", %{players: Player.all()})
    # broadcast(socket, "update_ranking", %{ranking: Ranking.all()})
  end

  defp format_state(state) do
    for {key, val} <- state, into: %{}, do: {String.to_atom(key), val}
  end
end
