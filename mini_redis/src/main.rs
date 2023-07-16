mod listener;
mod constants;
mod engine;

#[tokio::main]
async fn main() -> () {
    let db_listener = listener::Listener::new("127.0.0.1".to_string(), constants::DEFAULT_PORT);
    let tcp_listener = db_listener.run().await;
    let mut engine: engine::Engine = engine::Engine::new();

    // Standard Request: <method>:<key>:<value> 

    loop{
        let mut stream = listener::Listener::listen(&tcp_listener).await;
        let buffer = listener::Listener::read_request(&mut stream).await;
        println!("{:?}", buffer);
    }
}