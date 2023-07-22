mod listener;
mod constants;
mod engine;
mod writer;

#[tokio::main]
async fn main() -> () {
    let db_listener = listener::Listener::new("127.0.0.1".to_string(), constants::DEFAULT_PORT);
    let tcp_listener = db_listener.run().await;
    let mut engine: engine::Engine = engine::Engine::new();

    // Standard Request: <method>\0<key>\0<value> 
    // Standard Response: <type>\0<data>

    loop{
        let mut stream = listener::Listener::listen(&tcp_listener).await;
        let message = listener::Listener::read_request(&mut stream).await;
        let result = engine.process_message(&message);

        match result {
            Ok(result_message) => {
                println!("{}", result_message.data);
                let response = format!("{}{}{}", (result_message.response_type as u8) as char, constants::SEPERATOR as char, result_message.data);
                writer::write(stream, String::as_bytes(&response)).await;
            },
            Err(message) => {
                println!("{}", message);
                let response = format!("{}{}{}", (constants::ResponseType::Error as u8) as char, constants::SEPERATOR as char, message);
                writer::write(stream, String::as_bytes(&response)).await;
            }
        }
    }
}