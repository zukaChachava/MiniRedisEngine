mod listener;
mod constants;
mod engine;
mod writer;

#[tokio::main]
async fn main() -> () {
    let db_listener = listener::Listener::new("127.0.0.1".to_string(), constants::DEFAULT_PORT);
    let tcp_listener = db_listener.run().await;
    let mut engine: engine::Engine = engine::Engine::new();

    // Standard Request: <method>:<key>:<value> 
    // Standard Response: <type>:<data>

    loop{
        let mut stream = listener::Listener::listen(&tcp_listener).await;
        let message = listener::Listener::read_request(&mut stream).await;
        let result = engine.process_message(&message);

        match result {
            Ok(result_message) => {
                println!("{}", result_message);
                let response = format!("{}{}{}", constants::ResponseType::Data as i8, '\0', result_message);
                writer::write(stream, String::as_bytes(&response)).await;
            },
            Err(message) => {
                println!("{}", message);
                let response = format!("{}{}{}", constants::ResponseType::Error as i8, '\0', message);
                writer::write(stream, String::as_bytes(&response)).await;
            }
        }
    }
}