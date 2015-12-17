import javax.swing.*;

public class HelloJava3 {
	public static void main(String[] args) {
		JFrame frame = CreateFrame();
		frame.setVisible(true);
	}

	private static JFrame CreateFrame() {
		JFrame frame = new JFrame("Hello Java!");
		frame.add(new HelloComponent());
		frame.setSize(300, 300);

		return frame;
	}
}