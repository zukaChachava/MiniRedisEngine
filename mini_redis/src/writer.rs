use tokio::{net::{TcpSocket, TcpStream}, io::AsyncWriteExt};

pub async fn write(mut socket_stream: TcpStream, data: &[u8]){
    socket_stream.write(data).await.unwrap();
    socket_stream.flush().await.unwrap();
}