package be.infosupport.mcp_chess_demo;

import be.infosupport.mcp_chess_demo.model.ChessTools;
import java.util.List;
import org.springframework.ai.support.ToolCallbacks;
import org.springframework.ai.tool.ToolCallback;
import org.springframework.boot.SpringApplication;
import org.springframework.boot.autoconfigure.SpringBootApplication;
import org.springframework.context.annotation.Bean;

@SpringBootApplication
public class McpChessDemoApplication {

  public static void main(String[] args) {
    SpringApplication.run(McpChessDemoApplication.class, args);
  }

  @Bean
  public List<ToolCallback> toolCallBacks(ChessTools chessTools) {
    return List.of(ToolCallbacks.from(chessTools));
  }
}
