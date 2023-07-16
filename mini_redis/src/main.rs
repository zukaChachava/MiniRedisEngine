mod listener;

#[tokio::main]
async fn main() -> () {
    let db_listener = listener::Listener::new("127.0.0.1".to_string(), 9009);
    let tcp_listener = db_listener.run().await;

    loop{
        let mut stream = listener::Listener::listen(&tcp_listener).await;
        let buffer = listener::Listener::read_request_as_string(&mut stream).await;
        println!("{}", buffer);
    }
}