import { useEffect, useState } from "react";
import { apiClient } from "./clients/AxiosClient";

type HelloResponse = {
  message: string;
};

export default function App() {
  const [message, setMessage] = useState("Betöltés...");

  useEffect(() => {
    apiClient.get<HelloResponse>("/hello")
      .then((res) => setMessage(res.data.message))
      .catch((err) => {
        console.error(err);
        setMessage("Hiba történt");
      });
  }, []);

  return <h1>{message}</h1>;
}