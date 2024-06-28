import { View, Text } from "react-native";
import styles from "./HostPageStyles";
import { PlayPages } from "../../PlayPages";

interface HostPageProps {
  setPage: React.Dispatch<React.SetStateAction<PlayPages>>;
}

export default function HostPage({ setPage }: HostPageProps) {
  return (
    <View style={styles.container}>
      <Text style={styles.header}>HostPage</Text>
    </View>
  );
}
