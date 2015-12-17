import javax.swing.*;

public class HelloJava2 {
	public static void main(String[] args) {
		JFrame frame = CreateFrame();
		frame.setVisible(true);
	}

	private static JFrame CreateFrame() {
		JFrame frame = new JFrame("Hello Java!");
		JLabel label = new JLabel("Hello again!", JLabel.CENTER);
		frame.add(label);
		frame.setSize(300, 300);

		return frame;
	}
}