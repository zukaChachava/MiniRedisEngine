use tokio::{net::{TcpListener, TcpStream}, io::AsyncReadExt};

pub struct Listener{
    pub path: String,
    pub port: i32
}

impl Listener{
    pub fn new(path: String, port: i32) -> Listener{
        Listener {path, port}
    }

    pub async fn run(&self) -> TcpListener {
        TcpListener::bind(format!("{}:{}", self.path, self.port)).await.unwrap()
    }

    pub async fn listen(tcp_listener: &TcpListener) -> TcpStream{
        let (socket, _) = tcp_listener.accept().await.unwrap();
        socket
    }

    pub async fn read_request(stream: &mut TcpStream) -> [u8; 256]{
        let mut buf: [u8; 256] = [0; 256];
        stream.read(&mut buf).await.unwrap();
        buf
    }
}

