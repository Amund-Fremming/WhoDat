import { View, Text } from "react-native";
import styles from "./JoinPageStyles";
import { PlayPages } from "../../PlayPages";

interface JoinPageProps {
  setPage: React.Dispatch<React.SetStateAction<PlayPages>>;
}

export default function JoinPage({ setPage }: JoinPageProps) {
  return (
    <View style={styles.container}>
      <Text style={styles.header}>JoinPage</Text>
    </View>
  );
}
