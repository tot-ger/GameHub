import { useState, useEffect, useRef } from "react";
import { Routes, Route, useNavigate, useParams } from "react-router-dom";

type GameInfo = {
  id: string;
  player: string;
}

function App() {
  return (
    <Routes>
      <Route index element={<Home />} />
      <Route path="lobby" element={<Lobby />} />
      <Route path="game/:id" element={<Game />} />
    </Routes>
  )
}

function Home() {
  return(<></>)
}

function Lobby() {
  return (
    <></>
  )
}

function Game() {
  const { id } = useParams();
  return (<></>)
}

export default App
