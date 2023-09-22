defmodule ServerWeb.UserSocket do
  use Phoenix.Socket

  channel("game:*", ServerWeb.GameChannel)

  def connect(params, socket, _connect_info) do
    player_id = get_player_id(params["player_id"])
    nickname = get_nickname(params["nickname"])

    socket =
      socket
      |> assign(:nickname, nickname)
      |> assign(:player_id, player_id)

    IO.puts("#{IO.ANSI.yellow()}player_id: #{player_id}\nnickname: #{nickname}#{IO.ANSI.white()}")
    {:ok, socket}
  end

  def id(socket), do: "users_socket:#{socket.assigns.player_id}"

  defp get_player_id(player_id) do
    if player_id, do: player_id, else: UUID.uuid4()
  end

  defp get_nickname(nickname) do
    if nickname && String.first(nickname), do: nickname, else: UUID.uuid4()
  end
end
